﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.269
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 4.0.50826.0
// 
namespace PoliceSMS.ReportService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ReportService.IReportWcf")]
    public interface IReportWcf {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IReportWcf/LoadStationReportResult", ReplyAction="http://tempuri.org/IReportWcf/LoadStationReportResultResponse")]
        System.IAsyncResult BeginLoadStationReportResult(int UnitType, System.DateTime beginTime1, System.DateTime endTime1, System.DateTime beginTime2, System.DateTime endTime2, System.AsyncCallback callback, object asyncState);
        
        string EndLoadStationReportResult(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IReportWcf/LoadOfficerReportResult", ReplyAction="http://tempuri.org/IReportWcf/LoadOfficerReportResultResponse")]
        System.IAsyncResult BeginLoadOfficerReportResult(int UnitId, System.DateTime beginTime1, System.DateTime endTime1, System.DateTime beginTime2, System.DateTime endTime2, string officerName, System.AsyncCallback callback, object asyncState);
        
        string EndLoadOfficerReportResult(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IReportWcf/LoadOfficerByOrderReportResult", ReplyAction="http://tempuri.org/IReportWcf/LoadOfficerByOrderReportResultResponse")]
        System.IAsyncResult BeginLoadOfficerByOrderReportResult(int UnitId, System.DateTime beginTime1, System.DateTime endTime1, string officerName, int officerType, int orderIndex, System.AsyncCallback callback, object asyncState);
        
        string EndLoadOfficerByOrderReportResult(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IReportWcf/LoadTotalReportResult", ReplyAction="http://tempuri.org/IReportWcf/LoadTotalReportResultResponse")]
        System.IAsyncResult BeginLoadTotalReportResult(int start, int end, System.AsyncCallback callback, object asyncState);
        
        string EndLoadTotalReportResult(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IReportWcfChannel : PoliceSMS.ReportService.IReportWcf, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LoadStationReportResultCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public LoadStationReportResultCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LoadOfficerReportResultCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public LoadOfficerReportResultCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LoadOfficerByOrderReportResultCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public LoadOfficerByOrderReportResultCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LoadTotalReportResultCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public LoadTotalReportResultCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ReportWcfClient : System.ServiceModel.ClientBase<PoliceSMS.ReportService.IReportWcf>, PoliceSMS.ReportService.IReportWcf {
        
        private BeginOperationDelegate onBeginLoadStationReportResultDelegate;
        
        private EndOperationDelegate onEndLoadStationReportResultDelegate;
        
        private System.Threading.SendOrPostCallback onLoadStationReportResultCompletedDelegate;
        
        private BeginOperationDelegate onBeginLoadOfficerReportResultDelegate;
        
        private EndOperationDelegate onEndLoadOfficerReportResultDelegate;
        
        private System.Threading.SendOrPostCallback onLoadOfficerReportResultCompletedDelegate;
        
        private BeginOperationDelegate onBeginLoadOfficerByOrderReportResultDelegate;
        
        private EndOperationDelegate onEndLoadOfficerByOrderReportResultDelegate;
        
        private System.Threading.SendOrPostCallback onLoadOfficerByOrderReportResultCompletedDelegate;
        
        private BeginOperationDelegate onBeginLoadTotalReportResultDelegate;
        
        private EndOperationDelegate onEndLoadTotalReportResultDelegate;
        
        private System.Threading.SendOrPostCallback onLoadTotalReportResultCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public ReportWcfClient() {
        }
        
        public ReportWcfClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ReportWcfClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ReportWcfClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ReportWcfClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("无法设置 CookieContainer。请确保绑定包含 HttpCookieContainerBindingElement。");
                }
            }
        }
        
        public event System.EventHandler<LoadStationReportResultCompletedEventArgs> LoadStationReportResultCompleted;
        
        public event System.EventHandler<LoadOfficerReportResultCompletedEventArgs> LoadOfficerReportResultCompleted;
        
        public event System.EventHandler<LoadOfficerByOrderReportResultCompletedEventArgs> LoadOfficerByOrderReportResultCompleted;
        
        public event System.EventHandler<LoadTotalReportResultCompletedEventArgs> LoadTotalReportResultCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult PoliceSMS.ReportService.IReportWcf.BeginLoadStationReportResult(int UnitType, System.DateTime beginTime1, System.DateTime endTime1, System.DateTime beginTime2, System.DateTime endTime2, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginLoadStationReportResult(UnitType, beginTime1, endTime1, beginTime2, endTime2, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        string PoliceSMS.ReportService.IReportWcf.EndLoadStationReportResult(System.IAsyncResult result) {
            return base.Channel.EndLoadStationReportResult(result);
        }
        
        private System.IAsyncResult OnBeginLoadStationReportResult(object[] inValues, System.AsyncCallback callback, object asyncState) {
            int UnitType = ((int)(inValues[0]));
            System.DateTime beginTime1 = ((System.DateTime)(inValues[1]));
            System.DateTime endTime1 = ((System.DateTime)(inValues[2]));
            System.DateTime beginTime2 = ((System.DateTime)(inValues[3]));
            System.DateTime endTime2 = ((System.DateTime)(inValues[4]));
            return ((PoliceSMS.ReportService.IReportWcf)(this)).BeginLoadStationReportResult(UnitType, beginTime1, endTime1, beginTime2, endTime2, callback, asyncState);
        }
        
        private object[] OnEndLoadStationReportResult(System.IAsyncResult result) {
            string retVal = ((PoliceSMS.ReportService.IReportWcf)(this)).EndLoadStationReportResult(result);
            return new object[] {
                    retVal};
        }
        
        private void OnLoadStationReportResultCompleted(object state) {
            if ((this.LoadStationReportResultCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.LoadStationReportResultCompleted(this, new LoadStationReportResultCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void LoadStationReportResultAsync(int UnitType, System.DateTime beginTime1, System.DateTime endTime1, System.DateTime beginTime2, System.DateTime endTime2) {
            this.LoadStationReportResultAsync(UnitType, beginTime1, endTime1, beginTime2, endTime2, null);
        }
        
        public void LoadStationReportResultAsync(int UnitType, System.DateTime beginTime1, System.DateTime endTime1, System.DateTime beginTime2, System.DateTime endTime2, object userState) {
            if ((this.onBeginLoadStationReportResultDelegate == null)) {
                this.onBeginLoadStationReportResultDelegate = new BeginOperationDelegate(this.OnBeginLoadStationReportResult);
            }
            if ((this.onEndLoadStationReportResultDelegate == null)) {
                this.onEndLoadStationReportResultDelegate = new EndOperationDelegate(this.OnEndLoadStationReportResult);
            }
            if ((this.onLoadStationReportResultCompletedDelegate == null)) {
                this.onLoadStationReportResultCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnLoadStationReportResultCompleted);
            }
            base.InvokeAsync(this.onBeginLoadStationReportResultDelegate, new object[] {
                        UnitType,
                        beginTime1,
                        endTime1,
                        beginTime2,
                        endTime2}, this.onEndLoadStationReportResultDelegate, this.onLoadStationReportResultCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult PoliceSMS.ReportService.IReportWcf.BeginLoadOfficerReportResult(int UnitId, System.DateTime beginTime1, System.DateTime endTime1, System.DateTime beginTime2, System.DateTime endTime2, string officerName, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginLoadOfficerReportResult(UnitId, beginTime1, endTime1, beginTime2, endTime2, officerName, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        string PoliceSMS.ReportService.IReportWcf.EndLoadOfficerReportResult(System.IAsyncResult result) {
            return base.Channel.EndLoadOfficerReportResult(result);
        }
        
        private System.IAsyncResult OnBeginLoadOfficerReportResult(object[] inValues, System.AsyncCallback callback, object asyncState) {
            int UnitId = ((int)(inValues[0]));
            System.DateTime beginTime1 = ((System.DateTime)(inValues[1]));
            System.DateTime endTime1 = ((System.DateTime)(inValues[2]));
            System.DateTime beginTime2 = ((System.DateTime)(inValues[3]));
            System.DateTime endTime2 = ((System.DateTime)(inValues[4]));
            string officerName = ((string)(inValues[5]));
            return ((PoliceSMS.ReportService.IReportWcf)(this)).BeginLoadOfficerReportResult(UnitId, beginTime1, endTime1, beginTime2, endTime2, officerName, callback, asyncState);
        }
        
        private object[] OnEndLoadOfficerReportResult(System.IAsyncResult result) {
            string retVal = ((PoliceSMS.ReportService.IReportWcf)(this)).EndLoadOfficerReportResult(result);
            return new object[] {
                    retVal};
        }
        
        private void OnLoadOfficerReportResultCompleted(object state) {
            if ((this.LoadOfficerReportResultCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.LoadOfficerReportResultCompleted(this, new LoadOfficerReportResultCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void LoadOfficerReportResultAsync(int UnitId, System.DateTime beginTime1, System.DateTime endTime1, System.DateTime beginTime2, System.DateTime endTime2, string officerName) {
            this.LoadOfficerReportResultAsync(UnitId, beginTime1, endTime1, beginTime2, endTime2, officerName, null);
        }
        
        public void LoadOfficerReportResultAsync(int UnitId, System.DateTime beginTime1, System.DateTime endTime1, System.DateTime beginTime2, System.DateTime endTime2, string officerName, object userState) {
            if ((this.onBeginLoadOfficerReportResultDelegate == null)) {
                this.onBeginLoadOfficerReportResultDelegate = new BeginOperationDelegate(this.OnBeginLoadOfficerReportResult);
            }
            if ((this.onEndLoadOfficerReportResultDelegate == null)) {
                this.onEndLoadOfficerReportResultDelegate = new EndOperationDelegate(this.OnEndLoadOfficerReportResult);
            }
            if ((this.onLoadOfficerReportResultCompletedDelegate == null)) {
                this.onLoadOfficerReportResultCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnLoadOfficerReportResultCompleted);
            }
            base.InvokeAsync(this.onBeginLoadOfficerReportResultDelegate, new object[] {
                        UnitId,
                        beginTime1,
                        endTime1,
                        beginTime2,
                        endTime2,
                        officerName}, this.onEndLoadOfficerReportResultDelegate, this.onLoadOfficerReportResultCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult PoliceSMS.ReportService.IReportWcf.BeginLoadOfficerByOrderReportResult(int UnitId, System.DateTime beginTime1, System.DateTime endTime1, string officerName, int officerType, int orderIndex, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginLoadOfficerByOrderReportResult(UnitId, beginTime1, endTime1, officerName, officerType, orderIndex, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        string PoliceSMS.ReportService.IReportWcf.EndLoadOfficerByOrderReportResult(System.IAsyncResult result) {
            return base.Channel.EndLoadOfficerByOrderReportResult(result);
        }
        
        private System.IAsyncResult OnBeginLoadOfficerByOrderReportResult(object[] inValues, System.AsyncCallback callback, object asyncState) {
            int UnitId = ((int)(inValues[0]));
            System.DateTime beginTime1 = ((System.DateTime)(inValues[1]));
            System.DateTime endTime1 = ((System.DateTime)(inValues[2]));
            string officerName = ((string)(inValues[3]));
            int officerType = ((int)(inValues[4]));
            int orderIndex = ((int)(inValues[5]));
            return ((PoliceSMS.ReportService.IReportWcf)(this)).BeginLoadOfficerByOrderReportResult(UnitId, beginTime1, endTime1, officerName, officerType, orderIndex, callback, asyncState);
        }
        
        private object[] OnEndLoadOfficerByOrderReportResult(System.IAsyncResult result) {
            string retVal = ((PoliceSMS.ReportService.IReportWcf)(this)).EndLoadOfficerByOrderReportResult(result);
            return new object[] {
                    retVal};
        }
        
        private void OnLoadOfficerByOrderReportResultCompleted(object state) {
            if ((this.LoadOfficerByOrderReportResultCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.LoadOfficerByOrderReportResultCompleted(this, new LoadOfficerByOrderReportResultCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void LoadOfficerByOrderReportResultAsync(int UnitId, System.DateTime beginTime1, System.DateTime endTime1, string officerName, int officerType, int orderIndex) {
            this.LoadOfficerByOrderReportResultAsync(UnitId, beginTime1, endTime1, officerName, officerType, orderIndex, null);
        }
        
        public void LoadOfficerByOrderReportResultAsync(int UnitId, System.DateTime beginTime1, System.DateTime endTime1, string officerName, int officerType, int orderIndex, object userState) {
            if ((this.onBeginLoadOfficerByOrderReportResultDelegate == null)) {
                this.onBeginLoadOfficerByOrderReportResultDelegate = new BeginOperationDelegate(this.OnBeginLoadOfficerByOrderReportResult);
            }
            if ((this.onEndLoadOfficerByOrderReportResultDelegate == null)) {
                this.onEndLoadOfficerByOrderReportResultDelegate = new EndOperationDelegate(this.OnEndLoadOfficerByOrderReportResult);
            }
            if ((this.onLoadOfficerByOrderReportResultCompletedDelegate == null)) {
                this.onLoadOfficerByOrderReportResultCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnLoadOfficerByOrderReportResultCompleted);
            }
            base.InvokeAsync(this.onBeginLoadOfficerByOrderReportResultDelegate, new object[] {
                        UnitId,
                        beginTime1,
                        endTime1,
                        officerName,
                        officerType,
                        orderIndex}, this.onEndLoadOfficerByOrderReportResultDelegate, this.onLoadOfficerByOrderReportResultCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult PoliceSMS.ReportService.IReportWcf.BeginLoadTotalReportResult(int start, int end, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginLoadTotalReportResult(start, end, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        string PoliceSMS.ReportService.IReportWcf.EndLoadTotalReportResult(System.IAsyncResult result) {
            return base.Channel.EndLoadTotalReportResult(result);
        }
        
        private System.IAsyncResult OnBeginLoadTotalReportResult(object[] inValues, System.AsyncCallback callback, object asyncState) {
            int start = ((int)(inValues[0]));
            int end = ((int)(inValues[1]));
            return ((PoliceSMS.ReportService.IReportWcf)(this)).BeginLoadTotalReportResult(start, end, callback, asyncState);
        }
        
        private object[] OnEndLoadTotalReportResult(System.IAsyncResult result) {
            string retVal = ((PoliceSMS.ReportService.IReportWcf)(this)).EndLoadTotalReportResult(result);
            return new object[] {
                    retVal};
        }
        
        private void OnLoadTotalReportResultCompleted(object state) {
            if ((this.LoadTotalReportResultCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.LoadTotalReportResultCompleted(this, new LoadTotalReportResultCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void LoadTotalReportResultAsync(int start, int end) {
            this.LoadTotalReportResultAsync(start, end, null);
        }
        
        public void LoadTotalReportResultAsync(int start, int end, object userState) {
            if ((this.onBeginLoadTotalReportResultDelegate == null)) {
                this.onBeginLoadTotalReportResultDelegate = new BeginOperationDelegate(this.OnBeginLoadTotalReportResult);
            }
            if ((this.onEndLoadTotalReportResultDelegate == null)) {
                this.onEndLoadTotalReportResultDelegate = new EndOperationDelegate(this.OnEndLoadTotalReportResult);
            }
            if ((this.onLoadTotalReportResultCompletedDelegate == null)) {
                this.onLoadTotalReportResultCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnLoadTotalReportResultCompleted);
            }
            base.InvokeAsync(this.onBeginLoadTotalReportResultDelegate, new object[] {
                        start,
                        end}, this.onEndLoadTotalReportResultDelegate, this.onLoadTotalReportResultCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override PoliceSMS.ReportService.IReportWcf CreateChannel() {
            return new ReportWcfClientChannel(this);
        }
        
        private class ReportWcfClientChannel : ChannelBase<PoliceSMS.ReportService.IReportWcf>, PoliceSMS.ReportService.IReportWcf {
            
            public ReportWcfClientChannel(System.ServiceModel.ClientBase<PoliceSMS.ReportService.IReportWcf> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginLoadStationReportResult(int UnitType, System.DateTime beginTime1, System.DateTime endTime1, System.DateTime beginTime2, System.DateTime endTime2, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[5];
                _args[0] = UnitType;
                _args[1] = beginTime1;
                _args[2] = endTime1;
                _args[3] = beginTime2;
                _args[4] = endTime2;
                System.IAsyncResult _result = base.BeginInvoke("LoadStationReportResult", _args, callback, asyncState);
                return _result;
            }
            
            public string EndLoadStationReportResult(System.IAsyncResult result) {
                object[] _args = new object[0];
                string _result = ((string)(base.EndInvoke("LoadStationReportResult", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginLoadOfficerReportResult(int UnitId, System.DateTime beginTime1, System.DateTime endTime1, System.DateTime beginTime2, System.DateTime endTime2, string officerName, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[6];
                _args[0] = UnitId;
                _args[1] = beginTime1;
                _args[2] = endTime1;
                _args[3] = beginTime2;
                _args[4] = endTime2;
                _args[5] = officerName;
                System.IAsyncResult _result = base.BeginInvoke("LoadOfficerReportResult", _args, callback, asyncState);
                return _result;
            }
            
            public string EndLoadOfficerReportResult(System.IAsyncResult result) {
                object[] _args = new object[0];
                string _result = ((string)(base.EndInvoke("LoadOfficerReportResult", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginLoadOfficerByOrderReportResult(int UnitId, System.DateTime beginTime1, System.DateTime endTime1, string officerName, int officerType, int orderIndex, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[6];
                _args[0] = UnitId;
                _args[1] = beginTime1;
                _args[2] = endTime1;
                _args[3] = officerName;
                _args[4] = officerType;
                _args[5] = orderIndex;
                System.IAsyncResult _result = base.BeginInvoke("LoadOfficerByOrderReportResult", _args, callback, asyncState);
                return _result;
            }
            
            public string EndLoadOfficerByOrderReportResult(System.IAsyncResult result) {
                object[] _args = new object[0];
                string _result = ((string)(base.EndInvoke("LoadOfficerByOrderReportResult", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginLoadTotalReportResult(int start, int end, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[2];
                _args[0] = start;
                _args[1] = end;
                System.IAsyncResult _result = base.BeginInvoke("LoadTotalReportResult", _args, callback, asyncState);
                return _result;
            }
            
            public string EndLoadTotalReportResult(System.IAsyncResult result) {
                object[] _args = new object[0];
                string _result = ((string)(base.EndInvoke("LoadTotalReportResult", _args, result)));
                return _result;
            }
        }
    }
}
