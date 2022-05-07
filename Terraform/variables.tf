variable "azure_subscription_id" {
  type        = string
  description = "Azure subscription id"
}

variable "azure_tenant_id" {
  type        = string
  description = "Azure tenant id"
}

variable "azure_client_id" {
  type        = string
  description = "Azure service principal (app registration) client id"
}

variable "azure_client_secret" {
  type        = string
  description = "Azure service principal (app registration) client secret"
}

variable "env" {
  type        = string
  description = "Environment"
  default     = "dev"
}