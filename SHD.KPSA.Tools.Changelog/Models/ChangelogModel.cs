﻿namespace SHD.KPSA.Tools.Changelog.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Reflection;
    using System.Windows.Controls;
    using System.Windows.Media;
    using KPSA.Utils;
    using Properties;
    using Utils;

    public class ChangelogModel
    {
        #region Properties
        /// <summary>
        /// Gets the Path to the changelog.
        /// </summary>
        public string ChangelogFile { get; } = Constants.AppPath + Settings.Default.PathToChangelog;

        /// <summary>
        /// Gets the date of the build.
        /// </summary>
        public string BuildNote
        {
            get
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                var date = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);

                return string.Format(Resources.LastChangeText, date);
            }
        }

        /// <summary>
        /// Creates a new ObservableCollection of Labels.
        /// </summary>
        public ObservableCollection<Label> ChangelogLines { get; set; } = new ObservableCollection<Label>();
        #endregion Properties

        #region Constructor
        /// <summary>
        /// Initialize a new instance of the ChangelogModel class.
        /// </summary>
        public ChangelogModel()
        {
            DesignChangelogContent();
        }
        #endregion Constructor

        #region Methods
        private void DesignChangelogContent()
        {
            if (!File.Exists(ChangelogFile)) return;

            foreach (var line in FilesHandler.Read(ChangelogFile))
            {
                if (line.Equals("# CHANGELOG")) continue;

                var label = new Label
                {
                    FontFamily = new FontFamily("Consolas"),
                    FontSize = 14
                };

                var tmpLine = line;

                if (tmpLine.EndsWith("<br />"))
                {
                    tmpLine = tmpLine.Remove(tmpLine.Length - 6, 6);
                }

                if (tmpLine.StartsWith("## "))
                {
                    tmpLine = tmpLine.Remove(0, 3);

                    label.FontSize = 18;
                }

                if (tmpLine.StartsWith("[ADD]"))
                {
                    label.Foreground = Brushes.Green;
                }
                else if (tmpLine.StartsWith("[FIX]"))
                {
                    label.Foreground = Brushes.Orange;
                }
                else if (tmpLine.StartsWith("[REMOVE]"))
                {
                    label.Foreground = Brushes.Red;
                }

                label.Content = tmpLine;

                ChangelogLines.Add(label);
            }
        }
        #endregion Methods
    }
}