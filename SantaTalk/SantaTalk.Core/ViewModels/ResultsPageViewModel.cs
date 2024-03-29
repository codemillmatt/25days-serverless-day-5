﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;
using SantaTalk.Core.Services;
using SantaTalk.Models;
using Xamarin.Forms.StateSquid;

namespace SantaTalk.Core.ViewModels
{
    public class ResultsPageViewModel : BaseViewModel
    {
        string kidsName;
        public string KidsName 
        {
            get => kidsName;
            set => SetProperty(ref kidsName, value);
        }

        string letterText;
        public string LetterText
        {
            get => letterText;
            set => SetProperty(ref letterText, value);
        }

        State currentState = State.Loading;
        public State CurrentState
        {
            get => currentState;
            set => SetProperty(ref currentState, value);
        }

        string detectedLanguage;
        public string DetectedLanguage
        {
            get => detectedLanguage;
            set => SetProperty(ref detectedLanguage, value);
        }

        string santasComment;
        public string SantasComment
        {
            get => santasComment;
            set => SetProperty(ref santasComment, value);
        }

        string giftDecision;
        public string GiftDecision
        {
            get => giftDecision;
            set => SetProperty(ref giftDecision, value);
        }
        
        public async Task SendLetterToSanta()
        {
            CurrentState = State.Loading;
           
            var letter = new SantaLetter
            {
                KidName = KidsName,
                LetterText = LetterText
            };

            var letterService = new LetterDeliveryService();
            var results = await letterService.WriteLetterToSanta(letter);
                        
            if (results.SentimentScore == -1)
            {
                CurrentState = State.Error;
                return;
            }

            var commentsService = new SantasCommentsService();
            var comments = commentsService.MakeGiftDecision(results);

            SantasComment = comments.SentimentInterpretation;
            GiftDecision = comments.GiftPrediction;
            DetectedLanguage = results.DetectedLanguage;

            CurrentState = State.Success;
        }

    }
}
