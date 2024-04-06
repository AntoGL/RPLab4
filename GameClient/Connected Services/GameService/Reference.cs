﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GameClient.GameService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="OperationResult", Namespace="http://schemas.datacontract.org/2004/07/GameClassLibrary.ClientServer")]
    [System.SerializableAttribute()]
    public partial struct OperationResult : System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MessageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool SuccessField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Message {
            get {
                return this.MessageField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageField, value) != true)) {
                    this.MessageField = value;
                    this.RaisePropertyChanged("Message");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Success {
            get {
                return this.SuccessField;
            }
            set {
                if ((this.SuccessField.Equals(value) != true)) {
                    this.SuccessField = value;
                    this.RaisePropertyChanged("Success");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GameState", Namespace="http://schemas.datacontract.org/2004/07/GameClassLibrary.GameObject")]
    public enum GameState : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        End = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Active = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Unknown = 2,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="GameService.IServer", CallbackContract=typeof(GameClient.GameService.IServerCallback))]
    public interface IServer {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/Start", ReplyAction="http://tempuri.org/IServer/StartResponse")]
        System.Guid Start();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/Start", ReplyAction="http://tempuri.org/IServer/StartResponse")]
        System.Threading.Tasks.Task<System.Guid> StartAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/Stop", ReplyAction="http://tempuri.org/IServer/StopResponse")]
        GameClient.GameService.OperationResult Stop(System.Guid id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/Stop", ReplyAction="http://tempuri.org/IServer/StopResponse")]
        System.Threading.Tasks.Task<GameClient.GameService.OperationResult> StopAsync(System.Guid id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/GetGame", ReplyAction="http://tempuri.org/IServer/GetGameResponse")]
        void GetGame(System.Guid userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/GetGame", ReplyAction="http://tempuri.org/IServer/GetGameResponse")]
        System.Threading.Tasks.Task GetGameAsync(System.Guid userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/MakeTurn", ReplyAction="http://tempuri.org/IServer/MakeTurnResponse")]
        GameClient.GameService.OperationResult MakeTurn(System.Guid sessionId, System.Guid userId, int x, int y);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/MakeTurn", ReplyAction="http://tempuri.org/IServer/MakeTurnResponse")]
        System.Threading.Tasks.Task<GameClient.GameService.OperationResult> MakeTurnAsync(System.Guid sessionId, System.Guid userId, int x, int y);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/GetGameMap", ReplyAction="http://tempuri.org/IServer/GetGameMapResponse")]
        string GetGameMap(System.Guid sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/GetGameMap", ReplyAction="http://tempuri.org/IServer/GetGameMapResponse")]
        System.Threading.Tasks.Task<string> GetGameMapAsync(System.Guid sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/GetGameState", ReplyAction="http://tempuri.org/IServer/GetGameStateResponse")]
        GameClient.GameService.GameState GetGameState(System.Guid sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/GetGameState", ReplyAction="http://tempuri.org/IServer/GetGameStateResponse")]
        System.Threading.Tasks.Task<GameClient.GameService.GameState> GetGameStateAsync(System.Guid sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/GetWinerGuid", ReplyAction="http://tempuri.org/IServer/GetWinerGuidResponse")]
        System.Guid GetWinerGuid(System.Guid Session);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/GetWinerGuid", ReplyAction="http://tempuri.org/IServer/GetWinerGuidResponse")]
        System.Threading.Tasks.Task<System.Guid> GetWinerGuidAsync(System.Guid Session);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/GetQuestion", ReplyAction="http://tempuri.org/IServer/GetQuestionResponse")]
        string GetQuestion();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/GetQuestion", ReplyAction="http://tempuri.org/IServer/GetQuestionResponse")]
        System.Threading.Tasks.Task<string> GetQuestionAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/SetAnswer", ReplyAction="http://tempuri.org/IServer/SetAnswerResponse")]
        void SetAnswer(System.Guid idSession, System.Guid idPlayer, string answer);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/SetAnswer", ReplyAction="http://tempuri.org/IServer/SetAnswerResponse")]
        System.Threading.Tasks.Task SetAnswerAsync(System.Guid idSession, System.Guid idPlayer, string answer);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServerCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServer/Turn")]
        void Turn(System.Guid sessionGuid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/Ping", ReplyAction="http://tempuri.org/IServer/PingResponse")]
        bool Ping();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServer/GetUserName", ReplyAction="http://tempuri.org/IServer/GetUserNameResponse")]
        string GetUserName();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServerChannel : GameClient.GameService.IServer, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServerClient : System.ServiceModel.DuplexClientBase<GameClient.GameService.IServer>, GameClient.GameService.IServer {
        
        public ServerClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public ServerClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public ServerClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ServerClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ServerClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public System.Guid Start() {
            return base.Channel.Start();
        }
        
        public System.Threading.Tasks.Task<System.Guid> StartAsync() {
            return base.Channel.StartAsync();
        }
        
        public GameClient.GameService.OperationResult Stop(System.Guid id) {
            return base.Channel.Stop(id);
        }
        
        public System.Threading.Tasks.Task<GameClient.GameService.OperationResult> StopAsync(System.Guid id) {
            return base.Channel.StopAsync(id);
        }
        
        public void GetGame(System.Guid userId) {
            base.Channel.GetGame(userId);
        }
        
        public System.Threading.Tasks.Task GetGameAsync(System.Guid userId) {
            return base.Channel.GetGameAsync(userId);
        }
        
        public GameClient.GameService.OperationResult MakeTurn(System.Guid sessionId, System.Guid userId, int x, int y) {
            return base.Channel.MakeTurn(sessionId, userId, x, y);
        }
        
        public System.Threading.Tasks.Task<GameClient.GameService.OperationResult> MakeTurnAsync(System.Guid sessionId, System.Guid userId, int x, int y) {
            return base.Channel.MakeTurnAsync(sessionId, userId, x, y);
        }
        
        public string GetGameMap(System.Guid sessionId) {
            return base.Channel.GetGameMap(sessionId);
        }
        
        public System.Threading.Tasks.Task<string> GetGameMapAsync(System.Guid sessionId) {
            return base.Channel.GetGameMapAsync(sessionId);
        }
        
        public GameClient.GameService.GameState GetGameState(System.Guid sessionId) {
            return base.Channel.GetGameState(sessionId);
        }
        
        public System.Threading.Tasks.Task<GameClient.GameService.GameState> GetGameStateAsync(System.Guid sessionId) {
            return base.Channel.GetGameStateAsync(sessionId);
        }
        
        public System.Guid GetWinerGuid(System.Guid Session) {
            return base.Channel.GetWinerGuid(Session);
        }
        
        public System.Threading.Tasks.Task<System.Guid> GetWinerGuidAsync(System.Guid Session) {
            return base.Channel.GetWinerGuidAsync(Session);
        }
        
        public string GetQuestion() {
            return base.Channel.GetQuestion();
        }
        
        public System.Threading.Tasks.Task<string> GetQuestionAsync() {
            return base.Channel.GetQuestionAsync();
        }
        
        public void SetAnswer(System.Guid idSession, System.Guid idPlayer, string answer) {
            base.Channel.SetAnswer(idSession, idPlayer, answer);
        }
        
        public System.Threading.Tasks.Task SetAnswerAsync(System.Guid idSession, System.Guid idPlayer, string answer) {
            return base.Channel.SetAnswerAsync(idSession, idPlayer, answer);
        }
    }
}
