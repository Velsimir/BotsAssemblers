using System;
using TMPro;
using UnityEngine;

namespace CodeBase.MainBase
{
    public class ResourceCollectorView : IDisposable
    {
        private readonly TMP_Text _textCounterValue;
        
        private readonly ResourceCollector _resourceCollector;

        public ResourceCollectorView(ResourceCollector resourceCollector, TMP_Text textCounterValue)
        {
            _resourceCollector = resourceCollector;
            _resourceCollector.ValueChanged += UpdateView;
            _textCounterValue = textCounterValue;
            _textCounterValue.text = "";
        }

        private void UpdateView()
        {
            _textCounterValue.text = _resourceCollector.CollectedResources.ToString();
        }

        public void Dispose()
        {
            _resourceCollector.ValueChanged -= UpdateView;
        }
    }
}