using UnityEngine;
using System.Collections;
using System;
using G4AW2.Events;
using G4AW2.Utils;

namespace G4AW2.GPS
{
	public enum LocationState {
		Disabled,
		TimedOut,
		Failed,
		Enabled,
		Stopped,
		Initializing
	}

	public class GPS : MonoBehaviour {
		public static GPS gpsObject;

		public int GPSUpdatesBeforeAverage = 5;
		public float desiredAccuracyInMeters = 1f;
		public float updateDistanceInMeters = 0f;
		public float timeBetweenChecks = 1f;

		public GameEvent GPSUpdated;

		[HideInInspector]
		// Approximate radius of the earth (in kilometers)
		public ObservedValue<LocationState> state;

		// Position on earth (in degrees)
		public float latitude;
		public float longitude;

		// Distance walked (in meters) since last update 
		public ObservedValue<float> deltaDistance;
		public float deltaTime;

		// Amount of times GPS has updated
		private int gpsUpdates = 1;

		// Timestamp of last data
		private double timestamp;

		// Total lat and long for averaging
		private float totalLat = 0;
		private float totalLong = 0;

		// Previous average lat and long
		private float prevLatitude, prevLongitude;
		public double timeOfLastDistanceUpdate;

		private const float EARTH_RADIUS = 6371;

		private bool initialized = false;
		private bool started = false;

		// Use this for initialization
		void Awake() {
			gpsObject = this;
			deltaDistance = new ObservedValue<float>(0);
			timeOfLastDistanceUpdate = DateTime.UtcNow.Second;
			state = new ObservedValue<LocationState>(LocationState.Initializing);
			latitude = 0f;
			longitude = 0f;
		}

		IEnumerator Start() {
			state.OnValueChange += () => {
				if (state.Value != LocationState.Enabled && state.Value != LocationState.Initializing) {
					print("Could not connect to GPS!");
					//PopUp.instance.showPopUp("Could not connect to GPS!", new string[] { "Okay" });
				}
			};

			yield return 0; // Delay for a frame so state.OnValueChange isn't called right away.
			yield return StartCoroutine(initializeGPS());

			if (state.Value == LocationState.Enabled) {
				initializeVariables();
			}
			started = true;
		}

		void initializeVariables() {
			gpsUpdates = 1;
			timestamp = Input.location.lastData.timestamp;
			latitude = Input.location.lastData.latitude;
			longitude = Input.location.lastData.longitude;
			prevLatitude = latitude;
			prevLongitude = longitude;
			totalLong = prevLongitude;
			totalLat = prevLatitude;
			DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			timeOfLastDistanceUpdate = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
		}

		void OnApplicationPause( bool pauseState ) {
			if (!started)
				return;

			if (pauseState) {
				initialized = false;
				Input.location.Stop();
				state.Value = LocationState.Initializing;
			} else {
				StartCoroutine(initializeGPS());
			}
		}

		IEnumerator initializeGPS() {
			if (initialized) {
				yield break;
			}

			if (!Input.location.isEnabledByUser) {
				state.Value = LocationState.Disabled;
				initialized = true;
				yield break;
			}

			Input.location.Start(desiredAccuracyInMeters, updateDistanceInMeters);

			int waitTime = 15;

			while (Input.location.status == LocationServiceStatus.Initializing && waitTime > 0) {
				yield return new WaitForSeconds(1);
				waitTime--;
			}

			if (waitTime == 0) {
				state.Value = LocationState.TimedOut;
			} else if (Input.location.status == LocationServiceStatus.Failed) {
				state.Value = LocationState.Failed;
			} else {
				state.Value = LocationState.Enabled;
				StartCoroutine(checkForUpdates());
			}
			initialized = true;
		}

		// The Haversine formula
		// Veness, C. (2014). Calculate distance, bearing and more between
		//  Latitude/Longitude points. Movable Type Scripts. Retrieved from
		//  http://www.movable-type.co.uk/scripts/latlong.html
		float Haversine( float lastLongitude, float lastLatitude, float currLongitude, float currLatitude ) {
			float deltaLatitude = (currLatitude - lastLatitude) * Mathf.Deg2Rad;
			float deltaLongitude = (currLongitude - lastLongitude) * Mathf.Deg2Rad;
			float a = Mathf.Pow(Mathf.Sin(deltaLatitude / 2), 2) +
			          Mathf.Cos(lastLatitude * Mathf.Deg2Rad) * Mathf.Cos(currLatitude * Mathf.Deg2Rad) *
			          Mathf.Pow(Mathf.Sin(deltaLongitude / 2), 2);
			float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
			return EARTH_RADIUS * c;
		}

		IEnumerator checkForUpdates() {
			while (state.Value == LocationState.Enabled) {
				if (timestamp == Input.location.lastData.timestamp) {
					yield return new WaitForSecondsRealtime(timeBetweenChecks);
					continue;
				}
				print("Update!");
				totalLat += Input.location.lastData.latitude;
				totalLong += Input.location.lastData.longitude;
				timestamp = Input.location.lastData.timestamp;
				gpsUpdates++;

				if (gpsUpdates == GPSUpdatesBeforeAverage) {
					longitude = totalLong / GPSUpdatesBeforeAverage;
					latitude = totalLat / GPSUpdatesBeforeAverage;

					updateChangeInTime();
					deltaDistance.Value = Haversine(prevLongitude, prevLatitude, longitude, latitude) * 1000f;

					prevLongitude = longitude;
					prevLatitude = latitude;

					gpsUpdates = 1;
					totalLong = Input.location.lastData.longitude;
					totalLat = Input.location.lastData.latitude;
				}

				latitude = Input.location.lastData.latitude;
				longitude = Input.location.lastData.longitude;
			}
		}

		private void updateChangeInTime() {
			DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			double currentTime = (DateTime.UtcNow - epochStart).TotalSeconds;
			deltaTime = (float)(currentTime - timeOfLastDistanceUpdate);
			timeOfLastDistanceUpdate = currentTime;
		}

		public string getGPSData() {

			string text;
			switch (state.Value) {
				case LocationState.Enabled:
					DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0,
						DateTimeKind.Utc);

					double curTime = (DateTime.UtcNow - epochStart).TotalSeconds;
					float timeChanged = (float)(curTime - timestamp);
					int timeChange = Mathf.CeilToInt(timeChanged);


					text = "Time since last update: " + timeChange + "\n" +
					       "Previous Latitude: " + prevLatitude + "\n" +
					       "Previous Longitude: " + prevLongitude + "\n" +
					       "Current Latitude: " + latitude + "\n" +
					       "Current Longitude: " + longitude + "\n" +
					       "Delta Distance: " + deltaDistance.Value + "\n" +
					       "GPS updates: " + gpsUpdates;

					break;
				case LocationState.Disabled:
					text = "GPS disabled";
					break;
				case LocationState.Failed:
					text = "GPS failed";
					break;
				case LocationState.TimedOut:
					text = "GPS timed out";
					break;
				case LocationState.Stopped:
					text = "GPS stopped";
					break;
				case LocationState.Initializing:
					text = "GPS initializing";
					break;
				default:
					text = "GPS error occurred";
					break;
			} // Switch stmt

			return text;

		} //getGPSData

	} // class

}
