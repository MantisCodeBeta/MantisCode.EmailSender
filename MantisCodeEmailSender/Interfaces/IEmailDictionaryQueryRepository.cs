﻿using MantisCode.EmailSender.Enums;
using MantisCode.EmailSender.Models;

namespace MantisCode.EmailSender.Interfaces;

public interface IEmailDictionaryQueryRepository
{
    Task<EmailDictionaries?> GetEmailDictionaryByIdAsync(int id, string connectionString, DatabaseProviderEnum provider);
    Task<bool> IsDictionaryTableCreatedAsync(string connectionString, DatabaseProviderEnum provider);
}
