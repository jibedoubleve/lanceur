﻿using Probel.Lanceur.Core.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Probel.Lanceur.Models
{
    public static class AliasTextModelHelper
    {
        #region Methods

        public static IEnumerable<AliasTextModel> Cast(this IEnumerable<Query> src)
        {
            var dst = new List<AliasTextModel>();
            foreach (var item in src)
            {
                dst.Add(new AliasTextModel(item));
            }
            return dst;
        }

        #endregion Methods
    }

    public class AliasTextModel : Query, INotifyPropertyChanged
    {
        #region Fields

        private ImageSource _image;

        #endregion Fields

        #region Constructors

        //TODO: refactor this as it is error prone. Use StructureMap 
        public AliasTextModel(Query src)
        {
            ExecutionCount = src.ExecutionCount;
            FileName = src.FileName;
            Image = null;
            Kind = src.Kind;
            Name = src.Name;
            IsExecutable = src.IsExecutable;
            IsPackaged = src.IsPackaged;
            UniqueIdentifier = src.UniqueIdentifier;
            Icon = src.Icon;
            Id = src.Id;
            SearchScore = src.SearchScore;
        }

        #endregion Constructors

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Properties

        public ImageSource Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Methods
    }
}