using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RGN.Impl.Firebase;
using RGN.Modules.Wallets;
using RGN.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RGN.Samples
{
    public sealed class WalletsExample : IUIScreen, System.IDisposable
    {
        [Header("Internal references")]
        [SerializeField] private Button _createWalletButton;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _scrollContentRectTrasform;
        [SerializeField] private CreateWalletDialog _createWalletDialog;
        [SerializeField] private LoadingIndicator _loadingIndicator;

        [Header("Prefabs")]
        [SerializeField] private WalletItem _walletItemPrefab;

        private List<WalletItem> _walletItems;

        public override void PreInit(IRGNFrame rgnFrame)
        {
            base.PreInit(rgnFrame);
            _createWalletDialog.Init(this);
            _createWalletButton.onClick.AddListener(OnCreateButtonClick);
            _createWalletButton.gameObject.SetActive(true);
            _loadingIndicator.SetEnabled(false);
            _walletItems = new List<WalletItem>();
            RGNCore.I.AuthenticationChanged += OnAuthenticationChangedAsync;
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _createWalletDialog.Dispose();
            _createWalletButton.onClick.RemoveListener(OnCreateButtonClick);
            RGNCore.I.AuthenticationChanged -= OnAuthenticationChangedAsync;
            DisposeWalletItems();
        }

        public Task<string> GetPrimaryWalletAddressAsync()
        {
            //var walletsData = await WalletsModule.I.GetUserWalletsAsync();
            //if (walletsData.wallets.Length == 0)
            //{
            //    return "Create Primary Wallet";
            //}
            //return walletsData.wallets[0].address;
            return Task.FromResult("Not implemented");
        }
        public Task<bool> DoesTheUserHasPrimaryWalletAddressAsync()
        {
            //var response = await WalletsModule.I.IsUserHavePrimaryWalletAsync();
            //return response.isUserHavePrimaryWallet;
            return Task.FromResult(false);
        }
        internal void SetUIInteractable(bool interactable)
        {
            _canvasGroup.interactable = interactable;
            _loadingIndicator.SetEnabled(!interactable);
        }
        internal Task ReloadWalletItemsAsync()
        {
            //DisposeWalletItems();
            //SetUIInteractable(false);
            //var walletsData = await WalletsModule.I.GetUserWalletsAsync();
            //var wallets = walletsData.wallets;
            //for (int i = 0; i < wallets.Length; ++i)
            //{
            //    WalletItem walletItem = Instantiate(_walletItemPrefab, _scrollContentRectTrasform);
            //    walletItem.Init(i, wallets[i]);
            //    _walletItems.Add(walletItem);
            //}
            //Vector2 sizeDelta = _scrollContentRectTrasform.sizeDelta;
            //_scrollContentRectTrasform.sizeDelta = new Vector2(sizeDelta.x, wallets.Length * _walletItemPrefab.GetHeight());
            //SetUIInteractable(true);
            //_createWalletButton.gameObject.SetActive(wallets.Length == 0);
            return Task.CompletedTask;
        }
        private void OnCreateButtonClick()
        {
            SetUIInteractable(false);
            WalletsModule.I.CreateWallet(OnCreateWalletCallback);
        }
        private void OnCreateWalletCallback(bool success)
        {
            Debug.Log("Create wallet callback success: " + success);
            SetUIInteractable(true);
            ToastMessage.I.ShowSuccess("Wallet successfully created!");
        }

        private void DisposeWalletItems()
        {
            if (_walletItems == null)
            {
                return;
            }
            for (int i = 0; i < _walletItems.Count; ++i)
            {
                _walletItems[i].Dispose();
            }
            _walletItems.Clear();
        }
        private async void OnAuthenticationChangedAsync(AuthState authState)
        {
            if (authState.LoginState == EnumLoginState.Success)
            {
                await ReloadWalletItemsAsync();
            }
        }
    }
}
