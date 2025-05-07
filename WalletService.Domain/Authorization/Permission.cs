using System;

namespace WalletService.Domain.Authorization
{
    [Flags]
    public enum Permission
    {
        None = 0,
        
        // Wallet Permissions
        ViewWallet = 1 << 0,
        CreateWallet = 1 << 1,
        UpdateWallet = 1 << 2,
        DeactivateWallet = 1 << 3,
        
        // Transaction Permissions
        ViewTransactions = 1 << 4,
        CreateTransaction = 1 << 5,
        ApproveTransaction = 1 << 6,
        CancelTransaction = 1 << 7,
        
        // Account Permissions
        ViewAccount = 1 << 8,
        CreateAccount = 1 << 9,
        UpdateAccount = 1 << 10,
        CloseAccount = 1 << 11,
        
        // Admin Permissions
        ManageUsers = 1 << 12,
        ManageRoles = 1 << 13,
        ViewAuditLogs = 1 << 14,
        ManageSettings = 1 << 15,
        
        // Risk Management Permissions
        ViewRiskProfile = 1 << 16,
        UpdateRiskLevel = 1 << 17,
        FreezeAccount = 1 << 18,
        UnfreezeAccount = 1 << 19,
        
        // Dispute Permissions
        ViewDisputes = 1 << 20,
        CreateDispute = 1 << 21,
        ResolveDispute = 1 << 22,
        EscalateDispute = 1 << 23,
        
        // Settlement Permissions
        ViewSettlements = 1 << 24,
        CreateSettlement = 1 << 25,
        ApproveSettlement = 1 << 26,
        CancelSettlement = 1 << 27,
        
        // Predefined Role Permissions
        User = ViewWallet | ViewTransactions | CreateTransaction | ViewAccount | CreateDispute | ViewDisputes,
        
        Admin = User | ManageUsers | ManageRoles | ViewAuditLogs | ManageSettings | 
               UpdateRiskLevel | FreezeAccount | UnfreezeAccount | ResolveDispute | 
               EscalateDispute | ApproveSettlement | CancelSettlement,
               
        SuperAdmin = ~None // All permissions
    }
}