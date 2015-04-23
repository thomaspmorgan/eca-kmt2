/* Creates Email addresses */
USE CE
GO

INSERT INTO eca_dev.eca_dev.dbo.emailaddress
  (emailaddresstypeid,address,contact_contactid,person_personid)
SELECT et.EmailAddressTypeId,ce.EMAIL_ADDRESS,NULL,pm.eca_personid
FROM ce_email ce
LEFT JOIN eca_dev.eca_dev.dbo.emailaddresstype et 
ON (UPPER(et.emailaddresstypename) = UPPER(ce.location_type))
LEFT JOIN [dbo].[CE_Person_Map_dev] pm ON (pm.ce_personid = ce.person_id)

GO