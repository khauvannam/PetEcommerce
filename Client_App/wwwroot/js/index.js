'use strict'
const {StorageSharedKeyCredential} = require("@azure/storage-blob");
const accountName = 'devstoreaccount1';
const accountKey = 'Eby8vdM02xNOcqFeq2/JtUty2Hhfrl5vRBV6Kftq=';
const sharedKeyCredential = new StorageSharedKeyCredential(accountName, accountKey)    