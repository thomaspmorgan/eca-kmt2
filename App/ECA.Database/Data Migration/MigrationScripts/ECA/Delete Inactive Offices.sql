/* Delete office records that are Inactive (marked as deleted) */
select * from program 
where Owner_OrganizationId in (select organizationid from organization where organizationtypeid = 1 and status = 'Inactive')

delete from program 
where Owner_OrganizationId in (select organizationid from organization where organizationtypeid = 1 and status = 'Inactive')

select * from organization where organizationtypeid = 1 and status = 'Inactive'

delete from organization where organizationtypeid = 1 and status = 'Inactive'