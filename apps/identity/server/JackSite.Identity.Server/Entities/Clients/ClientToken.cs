namespace JackSite.Identity.Server.Entities.Clients
{
    public class ClientToken : BaseEntity
    {
        public int IdentityTokenLifeCycle { get; private set; }
        public int AccessTokenLifeCycle { get; private set; }
        public int AuthorizationCodeLifeCycle { get; private set; }
        public int UnconditionalRefreshTokenLifeCycle { get; private set; }
        public int RollRefreshTokenLifeCycle { get; private set; }
        public RefreshTokenType RefreshTokenType { get; private set; }
        public bool IsUpdateTokenOnRefresh { get; private set; }
        public bool IsIncludeJWT { get; private set; }
        public bool IsAlwaysSeedClientDefine { get; private set; }
        public bool IsAlwaysIncludeUserDefine { get; private set; }
        public string ClientDefinePrefix { get; private set; } = "client_";
        public string? MainPartSlat { get; private set; }
        public ICollection<ClientCrossPolicy> ClientCrossPolicies { get; private set; }
        public ICollection<BaseProprietyEntity> ClientTokenDefine { get; private set; }
        public ICollection<SigningAlgorithm> AllowedIdentityTokenSigningAlgorithms { get; private set; }

        private ClientToken()
        {
            ClientCrossPolicies = [];
            ClientTokenDefine = [];
            AllowedIdentityTokenSigningAlgorithms = [];
        }

        public ClientToken(
            int identityTokenLifeCycle,
            int accessTokenLifeCycle,
            int authorizationCodeLifeCycle,
            int unconditionalRefreshTokenLifeCycle,
            int rollRefreshTokenLifeCycle,
            RefreshTokenType refreshTokenType,
            bool isUpdateTokenOnRefresh,
            bool isIncludeJWT,
            bool isAlwaysSeedClientDefine,
            bool isAlwaysIncludeUserDefine,
            string? mainPartSlat = null)
        {
            IdentityTokenLifeCycle = identityTokenLifeCycle;
            AccessTokenLifeCycle = accessTokenLifeCycle;
            AuthorizationCodeLifeCycle = authorizationCodeLifeCycle;
            UnconditionalRefreshTokenLifeCycle = unconditionalRefreshTokenLifeCycle;
            RollRefreshTokenLifeCycle = rollRefreshTokenLifeCycle;
            RefreshTokenType = refreshTokenType;
            IsUpdateTokenOnRefresh = isUpdateTokenOnRefresh;
            IsIncludeJWT = isIncludeJWT;
            IsAlwaysSeedClientDefine = isAlwaysSeedClientDefine;
            IsAlwaysIncludeUserDefine = isAlwaysIncludeUserDefine;
            MainPartSlat = mainPartSlat;

            ClientCrossPolicies = [];
            ClientTokenDefine = [];
            AllowedIdentityTokenSigningAlgorithms = [];
        }

        public void Update(
            int identityTokenLifeCycle,
            int accessTokenLifeCycle,
            int authorizationCodeLifeCycle,
            int unconditionalRefreshTokenLifeCycle,
            int rollRefreshTokenLifeCycle,
            RefreshTokenType refreshTokenType,
            bool isUpdateTokenOnRefresh,
            bool isIncludeJWT,
            bool isAlwaysSeedClientDefine,
            bool isAlwaysIncludeUserDefine,
            string? mainPartSlat = null)
        {
            IdentityTokenLifeCycle = identityTokenLifeCycle;
            AccessTokenLifeCycle = accessTokenLifeCycle;
            AuthorizationCodeLifeCycle = authorizationCodeLifeCycle;
            UnconditionalRefreshTokenLifeCycle = unconditionalRefreshTokenLifeCycle;
            RollRefreshTokenLifeCycle = rollRefreshTokenLifeCycle;
            RefreshTokenType = refreshTokenType;
            IsUpdateTokenOnRefresh = isUpdateTokenOnRefresh;
            IsIncludeJWT = isIncludeJWT;
            IsAlwaysSeedClientDefine = isAlwaysSeedClientDefine;
            IsAlwaysIncludeUserDefine = isAlwaysIncludeUserDefine;
            MainPartSlat = mainPartSlat;
        }

        public void AddClientCrossPolicy(ClientCrossPolicy policy)
        {
            if (ClientCrossPolicies.Any(p => p.Id == policy.Id))
            {
                throw new InvalidOperationException("Policy already exists.");
            }
            ClientCrossPolicies.Add(policy);
        }

        public void RemoveClientCrossPolicy(string policyId)
        {
            var policy = ClientCrossPolicies.FirstOrDefault(p => p.Id == policyId);
            if (policy == null)
            {
                throw new InvalidOperationException("Policy not found.");
            }
            ClientCrossPolicies.Remove(policy);
        }

        public void AddClientTokenDefine(BaseProprietyEntity tokenDefine)
        {
            if (ClientTokenDefine.Any(td => td.Id == tokenDefine.Id))
            {
                throw new InvalidOperationException("Token define already exists.");
            }
            ClientTokenDefine.Add(tokenDefine);
        }

        public void RemoveClientTokenDefine(string defineId)
        {
            var tokenDefine = ClientTokenDefine.FirstOrDefault(td => td.Id == defineId);
            if (tokenDefine == null)
            {
                throw new InvalidOperationException("Token define not found.");
            }
            ClientTokenDefine.Remove(tokenDefine);
        }

        public void AddAllowedIdentityTokenSigningAlgorithm(SigningAlgorithm algorithm)
        {
            if (AllowedIdentityTokenSigningAlgorithms.Any(a => a.Id == algorithm.Id))
            {
                throw new InvalidOperationException("Algorithm already exists.");
            }
            AllowedIdentityTokenSigningAlgorithms.Add(algorithm);
        }

        public void RemoveAllowedIdentityTokenSigningAlgorithm(int algorithmId)
        {
            var algorithm = AllowedIdentityTokenSigningAlgorithms.FirstOrDefault(a => a.Id == algorithmId);
            if (algorithm == null)
            {
                throw new InvalidOperationException("Algorithm not found.");
            }
            AllowedIdentityTokenSigningAlgorithms.Remove(algorithm);
        }
    }
}