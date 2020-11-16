using System;
using System.Collections.Generic;
using System.Text;

namespace SweatyBot.Models
{
    public class AudioFile
    {
        private string _fileName;
        private string _title;
        private bool _isNetwork;
        private bool _isDownloaded;

        public AudioFile()
        {
            _fileName = "";
            _title = "";
            _isNetwork = true;
            _isDownloaded = false;
        }

        public override string ToString()
        {
            return _title;
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public bool IsNetwork
        {
            get { return _isNetwork; }
            set { _isNetwork = value; }
        }

        public bool IsDownloaded
        {
            get { return _isDownloaded; }
            set { _isDownloaded = value; }
        }
    }
}
