﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project_01.ServiceReference2 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference2.PayCheckSoap")]
    public interface PayCheckSoap {
        
        // CODEGEN: Generating message contract since element name Number from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Pay", ReplyAction="*")]
        Project_01.ServiceReference2.PayResponse Pay(Project_01.ServiceReference2.PayRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Pay", ReplyAction="*")]
        System.Threading.Tasks.Task<Project_01.ServiceReference2.PayResponse> PayAsync(Project_01.ServiceReference2.PayRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PayRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Pay", Namespace="http://tempuri.org/", Order=0)]
        public Project_01.ServiceReference2.PayRequestBody Body;
        
        public PayRequest() {
        }
        
        public PayRequest(Project_01.ServiceReference2.PayRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class PayRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Number;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Owner_ID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string Provider;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string CVV;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string DateOfExpiration;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string Cost;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public string Company;
        
        public PayRequestBody() {
        }
        
        public PayRequestBody(string Number, string Owner_ID, string Provider, string CVV, string DateOfExpiration, string Cost, string Company) {
            this.Number = Number;
            this.Owner_ID = Owner_ID;
            this.Provider = Provider;
            this.CVV = CVV;
            this.DateOfExpiration = DateOfExpiration;
            this.Cost = Cost;
            this.Company = Company;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PayResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PayResponse", Namespace="http://tempuri.org/", Order=0)]
        public Project_01.ServiceReference2.PayResponseBody Body;
        
        public PayResponse() {
        }
        
        public PayResponse(Project_01.ServiceReference2.PayResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class PayResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public bool PayResult;
        
        public PayResponseBody() {
        }
        
        public PayResponseBody(bool PayResult) {
            this.PayResult = PayResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface PayCheckSoapChannel : Project_01.ServiceReference2.PayCheckSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PayCheckSoapClient : System.ServiceModel.ClientBase<Project_01.ServiceReference2.PayCheckSoap>, Project_01.ServiceReference2.PayCheckSoap {
        
        public PayCheckSoapClient() {
        }
        
        public PayCheckSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PayCheckSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PayCheckSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PayCheckSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Project_01.ServiceReference2.PayResponse Project_01.ServiceReference2.PayCheckSoap.Pay(Project_01.ServiceReference2.PayRequest request) {
            return base.Channel.Pay(request);
        }
        
        public bool Pay(string Number, string Owner_ID, string Provider, string CVV, string DateOfExpiration, string Cost, string Company) {
            Project_01.ServiceReference2.PayRequest inValue = new Project_01.ServiceReference2.PayRequest();
            inValue.Body = new Project_01.ServiceReference2.PayRequestBody();
            inValue.Body.Number = Number;
            inValue.Body.Owner_ID = Owner_ID;
            inValue.Body.Provider = Provider;
            inValue.Body.CVV = CVV;
            inValue.Body.DateOfExpiration = DateOfExpiration;
            inValue.Body.Cost = Cost;
            inValue.Body.Company = Company;
            Project_01.ServiceReference2.PayResponse retVal = ((Project_01.ServiceReference2.PayCheckSoap)(this)).Pay(inValue);
            return retVal.Body.PayResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Project_01.ServiceReference2.PayResponse> Project_01.ServiceReference2.PayCheckSoap.PayAsync(Project_01.ServiceReference2.PayRequest request) {
            return base.Channel.PayAsync(request);
        }
        
        public System.Threading.Tasks.Task<Project_01.ServiceReference2.PayResponse> PayAsync(string Number, string Owner_ID, string Provider, string CVV, string DateOfExpiration, string Cost, string Company) {
            Project_01.ServiceReference2.PayRequest inValue = new Project_01.ServiceReference2.PayRequest();
            inValue.Body = new Project_01.ServiceReference2.PayRequestBody();
            inValue.Body.Number = Number;
            inValue.Body.Owner_ID = Owner_ID;
            inValue.Body.Provider = Provider;
            inValue.Body.CVV = CVV;
            inValue.Body.DateOfExpiration = DateOfExpiration;
            inValue.Body.Cost = Cost;
            inValue.Body.Company = Company;
            return ((Project_01.ServiceReference2.PayCheckSoap)(this)).PayAsync(inValue);
        }
    }
}
