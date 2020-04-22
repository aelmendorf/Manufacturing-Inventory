﻿using DevExpress.Mvvm;
using ManufacturingInventory.Common.Application;
using ManufacturingInventory.Common.Application.UI.Services;
using ManufacturingInventory.Domain.Enums;
using ManufacturingInventory.Infrastructure.Model.Entities;
using Prism.Events;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ManufacturingInventory.Domain.DTOs;
using System;
using ManufacturingInventory.Common.Application.UI.ViewModels;
using System.Windows;
using System.Collections.Generic;
using ManufacturingInventory.Domain.Extensions;
using ManufacturingInventory.Application.Boundaries;
using System.Text;
using ManufacturingInventory.Infrastructure.Model;
using System.Diagnostics;
using System.IO;

namespace ManufacturingInventory.Reporting.ViewModels {
    public class ReportingMonthlySummaryViewModel : InventoryViewModelBase {
        protected IDispatcherService DispatcherService { get => ServiceContainer.GetService<IDispatcherService>("MonthlySummaryDispatcherService"); }
        protected IMessageBoxService MessageBoxService { get => ServiceContainer.GetService<IMessageBoxService>("MonthlySummaryMessageBoxService"); }
        protected IExportService ExportService { get => ServiceContainer.GetService<IExportService>("MonthlySummaryExportService"); }

        private IEventAggregator _eventAggregator;
        private IRegionManager _regionManager;
        private ManufacturingContext _context;

        private ObservableCollection<ReportSnapshot> _reportSnapshot;
        private DateTime _start;
        private DateTime _stop;
        private bool _showTableLoading;

        public AsyncCommand ExportTableCommand { get; private set; }
        public AsyncCommand CollectSnapshotCommand { get; private set; }
        public AsyncCommand InitializeCommand { get; private set;}

        public ReportingMonthlySummaryViewModel(IRegionManager regionManager,IEventAggregator eventAggregator,ManufacturingContext context) {
            this._context = context;
            this._eventAggregator = eventAggregator;
            this._regionManager = regionManager;
            this.Start = DateTime.Now;
            this.Stop = DateTime.Now;
            this.CollectSnapshotCommand = new AsyncCommand(this.CollectSummaryHandler);
        }

        public override bool KeepAlive => true;
        
        public ObservableCollection<ReportSnapshot> ReportSnapshot {
            get => this._reportSnapshot;
            set => SetProperty(ref this._reportSnapshot, value);
        }

        public DateTime Start {
            get => this._start;
            set => SetProperty(ref this._start, value);
        }

        public DateTime Stop {
            get => this._stop;
            set => SetProperty(ref this._stop, value);
        }

        public bool ShowTableLoading {
            get => this._showTableLoading;
            set => SetProperty(ref this._showTableLoading, value);
        }

        private Task CollectSummaryHandler() {
            return Task.CompletedTask;
        }

        private async Task ExportTableHandler(ExportFormat format) {
            await Task.Run(() => {
                var path = Path.ChangeExtension(Path.GetTempFileName(), format.ToString().ToLower());
                using (FileStream file = File.Create(path)) {
                    this.ExportService.Export(file, format);
                }
                using (var process = new Process()) {
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = path;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                }
            });
        }
    }
}