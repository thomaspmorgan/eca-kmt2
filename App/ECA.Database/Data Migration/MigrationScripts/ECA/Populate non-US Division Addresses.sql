/* Updates provinces */
UPDATE
    L
SET
    L.division = CA.province
FROM
    eca_dev.eca_dev.dbo.location L
INNER JOIN
    ce_address ca 
ON
    ca.ADDRESS_LINE1 = l.street1
and ((ca.address_line2 IS NULL and l.street2 is null) OR ca.ADDRESS_LINE2 = l.street2)
and ((ca.address_line3 IS NULL and l.street3 is null) OR ca.ADDRESS_LINE3 = l.street3)
and ((ca.city IS NULL and l.city is null) OR ca.city = l.city)
and ((ca.postal_cd is null and l.postalcode is null) OR ca.postal_cd = l.postalcode)

WHERE
    L.locationtypeid = 9 and L.street1 is not null and L.division is null