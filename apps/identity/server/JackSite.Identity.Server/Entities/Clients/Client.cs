namespace JackSite.Identity.Server.Entities.Clients
{
    public class Client : BaseEntity
    {
        public string ClientFlag { get; private set; }
        public string ClientName { get; private set; }
        public ClientType ClientType { get; private set; }
        public ClientBasic? ClientBasic { get; private set; }
        public ClientCertificationCancel? ClientCertificationCancel { get; private set; }
        public ClientToken? ClientToken { get; private set; }
        public ClientAllowScreen? ClientAllowScreen { get; private set; }
        public ClientDeviceWorkflow? ClientDeviceWorkflow { get; private set; }

        private Client()
        {
        }

        public Client(
            string clientFlag,
            string clientName,
            ClientType clientType,
            ClientBasic? clientBasic = null,
            ClientCertificationCancel? clientCertificationCancel = null,
            ClientToken? clientToken = null,
            ClientAllowScreen? clientAllowScreen = null,
            ClientDeviceWorkflow? clientDeviceWorkflow = null)
        {
            ClientFlag = clientFlag;
            ClientName = clientName;
            ClientType = clientType;
            ClientBasic = clientBasic;
            ClientCertificationCancel = clientCertificationCancel;
            ClientToken = clientToken;
            ClientAllowScreen = clientAllowScreen;
            ClientDeviceWorkflow = clientDeviceWorkflow;
        }

        public void Update(
            string clientFlag,
            string clientName,
            ClientType clientType,
            string UpdateBy,
            ClientBasic? clientBasic = null,
            ClientCertificationCancel? clientCertificationCancel = null,
            ClientToken? clientToken = null,
            ClientAllowScreen? clientAllowScreen = null,
            ClientDeviceWorkflow? clientDeviceWorkflow = null
            )
        {
            this.UpdateBy = UpdateBy;
            UpdateAt = DateTime.UtcNow;
            ClientFlag = clientFlag;
            ClientName = clientName;
            ClientType = clientType;
            ClientBasic = clientBasic;
            ClientCertificationCancel = clientCertificationCancel;
            ClientToken = clientToken;
            ClientAllowScreen = clientAllowScreen;
            ClientDeviceWorkflow = clientDeviceWorkflow;
        }


        public void SetClientBasic(ClientBasic clientBasic)
        {
            ClientBasic = clientBasic;
        }

        public void SetClientCertificationCancel(ClientCertificationCancel clientCertificationCancel)
        {
            ClientCertificationCancel = clientCertificationCancel;
        }

        public void SetClientToken(ClientToken clientToken)
        {
            ClientToken = clientToken;
        }

        public void SetClientAllowScreen(ClientAllowScreen clientAllowScreen)
        {
            ClientAllowScreen = clientAllowScreen;
        }

        public void SetClientDeviceWorkflow(ClientDeviceWorkflow clientDeviceWorkflow)
        {
            ClientDeviceWorkflow = clientDeviceWorkflow;
        }
    }
}