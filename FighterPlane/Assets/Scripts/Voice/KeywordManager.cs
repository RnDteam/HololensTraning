using HoloToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows.Speech;

namespace Academy.HoloToolkit.Unity
{
    /// <summary>
    /// KeywordManager allows you to specify keywords and methods in the Unity
    /// Inspector, instead of registering them explicitly in code.
    /// This also includes a setting to either automatically start the
    /// keyword recognizer or allow your code to start it.
    ///
    /// IMPORTANT: Please make sure to add the microphone capability in your app, in Unity under
    /// Edit -> Project Settings -> Player -> Settings for Windows Store -> Publishing Settings -> Capabilities
    /// or in your Visual Studio Package.appxmanifest capabilities.
    /// </summary>
    public partial class KeywordManager : Singleton<KeywordManager>
    { 

        [System.Serializable]
        public struct MyBetterKeywordAndResponse
        {
            public string MethodPurpose;
            [Tooltip("The keywords to recognize.")]
            public List<string> Keywords;
            [Tooltip("The UnityEvent to be invoked when the keyword is recognized.")]
            public UnityEvent Response;
        }

        // This enumeration gives the manager two different ways to handle the recognizer. Both will
        // set up the recognizer and add all keywords. The first causes the recognizer to start
        // immediately. The second allows the recognizer to be manually started at a later time.
        public enum RecognizerStartBehavior { AutoStart, ManualStart };

        [Tooltip("An enumeration to set whether the recognizer should start on or off.")]
        public RecognizerStartBehavior RecognizerStart;
        [Tooltip("An array of string keywords and UnityEvents, to be set in the Inspector.")]
        public MyBetterKeywordAndResponse[] myKeywordsAndResponses;

        private KeywordRecognizer keywordRecognizer;
        private Dictionary<string, UnityEvent> responses;

        // Convert the struct array into a dictionary, with the keywords and the keys and the methods as the values.
        // This helps easily link the keyword recognized to the UnityEvent to be invoked.
        private void InitializeResponsesDictionary()
        {
            responses = new Dictionary<string, UnityEvent>();

            if (myKeywordsAndResponses.Length > 0)
            {
                foreach (MyBetterKeywordAndResponse mykeywordAndResopnse in myKeywordsAndResponses)
                {
                    foreach (string keyword in mykeywordAndResopnse.Keywords)
                    {
                        responses.Add(keyword, mykeywordAndResopnse.Response);
                    }
                }
            }
        }

        void Start()
        {
            InitializeResponsesDictionary();
            StartRecognizer();
        }

        private void StartRecognizer()
        {
            if (myKeywordsAndResponses.Length > 0)
            {
                keywordRecognizer = new KeywordRecognizer(responses.Keys.ToArray());
                keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;

                if (RecognizerStart == RecognizerStartBehavior.AutoStart)
                {
                    keywordRecognizer.Start();
                }
            }
        }

        void Restart()
        {
            // InitializeResponsesDictionary();
            StartRecognizer();
        }

        void OnDestroy()
        {
            if (keywordRecognizer != null)
            {
                StopKeywordRecognizer();
                keywordRecognizer.OnPhraseRecognized -= KeywordRecognizer_OnPhraseRecognized;
                keywordRecognizer.Dispose();
            }
        }

        private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            UnityEvent keywordResponse;

            // Check to make sure the recognized keyword exists in the methods dictionary, then invoke the corresponding method.
            if (responses.TryGetValue(args.text, out keywordResponse))
            {
                Debug.Log(args.text);
                BuildingManager.Instance.BuildingKeyword = args.text;
                keywordResponse.Invoke();
            }
        }

        /// <summary>
        /// Make sure the keyword recognizer is off, then start it.
        /// Otherwise, leave it alone because it's already in the desired state.
        /// </summary>
        public void StartKeywordRecognizer()
        {
            if (keywordRecognizer != null && !keywordRecognizer.IsRunning)
            {
                keywordRecognizer.Start();
            }
        }

        /// <summary>
        /// Make sure the keyword recognizer is on, then stop it.
        /// Otherwise, leave it alone because it's already in the desired state.
        /// </summary>
        public void StopKeywordRecognizer()
        {
            if (keywordRecognizer != null && keywordRecognizer.IsRunning)
            {
                keywordRecognizer.Stop();
            }
        }

        public void AddKeywordAndResponse(string methodPurpose, List<string> lstKeywords, UnityEvent response)
        {
            if (myKeywordsAndResponses.Any(kar => kar.Keywords == lstKeywords))
            {
                return;
            }

            var NewKeywordsAndResponses = new MyBetterKeywordAndResponse[myKeywordsAndResponses.Length + 1];
            for (int i = 0; i < myKeywordsAndResponses.Length; i++)
            {
                NewKeywordsAndResponses[i] = myKeywordsAndResponses[i];
            }
            NewKeywordsAndResponses[myKeywordsAndResponses.Length] = new MyBetterKeywordAndResponse() { MethodPurpose = methodPurpose, Keywords = lstKeywords, Response = response };
            
            myKeywordsAndResponses = NewKeywordsAndResponses;

            //responses.Clear();

            //responses = new Dictionary<string, UnityEvent>();

            foreach (string keyword in lstKeywords)
            {
                responses.Add(keyword, response);
            }

            Restart();
        }

        public void RemoveKeyword(List<string> lstKeywords)
        {
            if (myKeywordsAndResponses.Any(kar => kar.Keywords == lstKeywords))
            {
                return;
            }

            var NewKeywordsAndResponses = new MyBetterKeywordAndResponse[myKeywordsAndResponses.Length - 1];
            for (int i = 0, j = 0; i < myKeywordsAndResponses.Length; i++)
            {
                if (myKeywordsAndResponses[i].Keywords != lstKeywords)
                    NewKeywordsAndResponses[j++] = myKeywordsAndResponses[i];
            }
            myKeywordsAndResponses = NewKeywordsAndResponses;
            Restart();
        }
    }
}