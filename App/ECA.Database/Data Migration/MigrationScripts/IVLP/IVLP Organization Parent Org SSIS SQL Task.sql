/* Update IVLP Organization Parent OrganizationId following load of new organizations */

/* Updates Parent OrganizationId for Organization table table */
UPDATE
    org
SET
    org.ParentOrganization_OrganizationId = x.ECA_ParentOrganization_OrganizationId
FROM
    dbo.Organization org
INNER JOIN
    IVLP_XREF.IVLP_XRef.dbo.Local_IVLP_Organization_XREF x 
ON
    x.ECA_OrganizationId = org.OrganizationId




