﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClientTest.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IService1")]
    public interface IService1 {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetTest", ReplyAction="http://tempuri.org/IService1/GetTestResponse")]
        string GetTest();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetTest", ReplyAction="http://tempuri.org/IService1/GetTestResponse")]
        System.Threading.Tasks.Task<string> GetTestAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetTestOK", ReplyAction="http://tempuri.org/IService1/GetTestOKResponse")]
        string GetTestOK();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetTestOK", ReplyAction="http://tempuri.org/IService1/GetTestOKResponse")]
        System.Threading.Tasks.Task<string> GetTestOKAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IService1Channel : ClientTest.ServiceReference1.IService1, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Service1Client : System.ServiceModel.ClientBase<ClientTest.ServiceReference1.IService1>, ClientTest.ServiceReference1.IService1 {
        
        public Service1Client() {
        }
        
        public Service1Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Service1Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetTest() {
            return base.Channel.GetTest();
        }
        
        public System.Threading.Tasks.Task<string> GetTestAsync() {
            return base.Channel.GetTestAsync();
        }
        
        public string GetTestOK() {
            return base.Channel.GetTestOK();
        }
        
        public System.Threading.Tasks.Task<string> GetTestOKAsync() {
            return base.Channel.GetTestOKAsync();
        }
    }
}
