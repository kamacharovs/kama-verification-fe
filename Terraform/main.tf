terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.1.0"
    }

    random = {
      source  = "hashicorp/random"
      version = "3.0.1"
    }
  }

  cloud {
    organization = "kamacharovs"

    workspaces {
      name = "kama-verification-fe"
    }
  }
}

provider "azurerm" {
  subscription_id = var.azure_subscription_id
  tenant_id       = var.azure_tenant_id
  client_id       = var.azure_client_id
  client_secret   = var.azure_client_secret

  features {}
}

locals {
  location = "eastus"
}

resource "azurerm_resource_group" "kama_verification_rg" {
  name     = "kama-verification-fe-rg"
  location = local.location
}