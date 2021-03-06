﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Prism.Common;
using Prism.Regions;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ManufacturingInventory.Common.Application.UI.Services;
using ManufacturingInventory.Common.Application.UI.ViewModels;

namespace ManufacturingInventory.Common.Application.UI.Views {
    /// <summary>
    /// Interaction logic for AttachmentsTableView.xaml
    /// </summary>
    public partial class AttachmentsTableView : UserControl {
        public AttachmentsTableView() {
            InitializeComponent();
            RegionContext.GetObservableContext(this).PropertyChanged += this.AttachmentsTableView_PropertyChanged;
        }

        private void AttachmentsTableView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            var context = (ObservableObject<object>)sender;
            var attachmentContext = (AttachmentDataTraveler)context.Value;
            (DataContext as AttachmentsTableViewModel).SelectedEntityId = attachmentContext.EntityId;
            (DataContext as AttachmentsTableViewModel).GetBy = attachmentContext.GetBy;
        }
    }
}
