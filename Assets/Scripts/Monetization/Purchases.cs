﻿using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using System.Collections.Generic;
using Assets.Scripts.Game;

namespace Assets.Scripts.Monetization
{
    class Purchases : MonoBehaviour, IStoreListener
    {

        public const string 
            DISABLE_ADS = "disable_ads",
            DONATE = "donate",
            SKIP = "unlock_skip_3_levels",
            UNLOCK_ALL = "unlock_all",
            UNLOCK_ALL_THEMES = "unlock_all_themes",
            UNLOCK_ENDLESS = "unlock_endless_mode",
            UNLOCK_CONFIGURABLE = "unlock_configurable_mode";

        private static IStoreController storeController = null;
        private static IExtensionProvider storeExtensionProvider = null;
        public static Purchases Instance
        {
            get; private set;
        }
        public static bool Ready
        {
            get { return Instance != null && Instance.Initialized; }
        }
        public class ProductDescriptor
        {
            public string id;
            public ProductType type;
            public ProductDescriptor(string id, ProductType type)
            {
                this.type = type;
                this.id = id;
            }
        }

        public System.Action<string> OnSuccess = id => Debug.LogFormat("Successfuly bought:{0}", id);
        public System.Action<string> OnFail = id => Debug.LogFormat("Failed to buy:{0}", id);

        [SerializeField]
        private List<ProductDescriptor> products = new List<ProductDescriptor>()
        {
            new ProductDescriptor(UNLOCK_ALL, ProductType.NonConsumable),
            new ProductDescriptor(UNLOCK_ALL_THEMES, ProductType.NonConsumable),
            new ProductDescriptor(DISABLE_ADS, ProductType.NonConsumable),
            new ProductDescriptor(UNLOCK_ENDLESS, ProductType.NonConsumable),
            new ProductDescriptor(UNLOCK_CONFIGURABLE, ProductType.NonConsumable),
            new ProductDescriptor(DONATE, ProductType.Consumable),
            new ProductDescriptor(SKIP, ProductType.Consumable)
        };
       
        private void Start()
        {
            if (storeController == null && Instance == null)
            {
                InitializePurchasing();
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private bool Initialized
        {
            get { return storeController != null && storeExtensionProvider != null; }
        }

        private void InitializePurchasing()
        {
            if (Initialized)
                return;
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            foreach (var product in products)
            {
                if (product.type != ProductType.Subscription)
                {
                    var ids = new IDs
                    {
                        { product.id, GooglePlay.Name }
                    };
                    builder.AddProduct(product.id, product.type, ids);
                }
                else
                {
                    throw new System.NotImplementedException("Subscription products are not implemented!");
                }
            }
            UnityPurchasing.Initialize(this, builder);
        }


        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("Purchases are initialized!");
            storeController = controller;
            storeExtensionProvider = extensions;
        }

        public void Buy(string id)
        {
            if (!Initialized)
            {
                Debug.Log("Fail! Purchases are not initialized!");
                return;
            }
            Product product = storeController.products.WithID(id);
            if(product == null)
            {
                Debug.Log("Product not found!");
                OnFail(null);
                return;
            }else if (!product.availableToPurchase)
            {
                Debug.Log("Not available to purchase!");
                OnFail(product.definition.id);
                return;
            }
            else
            {
                Debug.LogFormat("Purchasing product asynchronously:{0}", product.definition.id);
                storeController.InitiatePurchase(product);
            }

        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("Purchases initialization error! Error:"+error);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.LogFormat("Purchase Failed! Product:[{0}] Reason:[{1}]", product.definition.storeSpecificId, failureReason);
            OnFail(product.definition.storeSpecificId);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            var data = PersistentState.Instance.data;
            switch (args.purchasedProduct.definition.id)
            {
                case UNLOCK_ALL:
                    data.adsDisabled = true;
                    data.endlessModeUnlocked = true;
                    data.customModeUnlocked = true;
                    data.themesUnlocked = true;
                    break;
                case UNLOCK_ALL_THEMES:
                    data.themesUnlocked = true;
                    break;
                case DISABLE_ADS:
                    data.adsDisabled = true;
                    break;
                case UNLOCK_ENDLESS:
                    data.endlessModeUnlocked = true;
                    break;
                case UNLOCK_CONFIGURABLE:
                    data.customModeUnlocked = true;
                    break;
                case SKIP:
                    data.levelsUnlocked = Mathf.Clamp(data.levelsUnlocked+3,1, DifficultyLevels.Instance.LevelCount);
                    break;
                case DONATE:
                    break;
            }
            OnSuccess(args.purchasedProduct.definition.id);
            return PurchaseProcessingResult.Complete;
        }

        public void BuyUnlockAllThemes()
        {
            Buy(UNLOCK_ALL_THEMES);
        }

        public void BuyUnlockAll()
        {
            Buy(UNLOCK_ALL);
        }

        public void BuyDisableAds()
        {
            Buy(DISABLE_ADS);
        }

        public void BuySkipLevels()
        {
            Buy(SKIP);
        }

        public void BuyUnlockEndless()
        {
            Buy(UNLOCK_ENDLESS);
        }

        public void BuyUnlockConfigurable()
        {
            Buy(UNLOCK_CONFIGURABLE);
        }

        public void BuyDonate()
        {
            Buy(DONATE);
        }
    }
}
