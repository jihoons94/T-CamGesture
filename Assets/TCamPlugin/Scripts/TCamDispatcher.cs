using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


namespace TCamera {

	/// <summary>
	/// Unity 메인 쓰레드 디스패처
	/// </summary>
	public class TCamDispatcher : MonoBehaviour
	{
		private static readonly Queue<Action> _executionQueue = new Queue<Action>();
		private Thread mainThread;

		public void Update() {
			lock(_executionQueue) {
				while (_executionQueue.Count > 0) {
					_executionQueue.Dequeue().Invoke();
				}
			}
		}

		/// <summary>
		/// Locks the queue and adds the IEnumerator to the queue
		/// </summary>
		/// <param name="action">IEnumerator function that will be executed from the main thread.</param>
		public void Enqueue(IEnumerator action) {
			lock (_executionQueue) {
				_executionQueue.Enqueue (() => {
					StartCoroutine (action);
				});
			}
		}

		/// <summary>
		/// Locks the queue and adds the Action to the queue
		/// </summary>
		/// <param name="action">function that will be executed from the main thread.</param>
		public void Enqueue(Action action)
		{
			if (Thread.CurrentThread.ManagedThreadId == mainThread.ManagedThreadId) {
				action ();
			} else {
				Enqueue (ActionWrapper (action));
			}
		}

		IEnumerator ActionWrapper(Action a)
		{
			a();
			yield return null;
		}


		private static TCamDispatcher _instance = null;

		public static bool Exists() {
			return _instance != null;
		}

		public static TCamDispatcher Instance() {
			if (!Exists ()) {
				throw new Exception ("UnityMainThreadDispatcher could not find the UnityMainThreadDispatcher object. Please ensure you have added the MainThreadExecutor Prefab to your scene.");
			}
			return _instance;
		}


		void Awake() {
			if (_instance == null) {
				_instance = this;
				DontDestroyOnLoad(this.gameObject);

				mainThread = Thread.CurrentThread;
			}
		}

		void OnDestroy() {
			_instance = null;
		}

    }

}
