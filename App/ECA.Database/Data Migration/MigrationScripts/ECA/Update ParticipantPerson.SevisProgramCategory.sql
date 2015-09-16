/* Updates programcategoryid for participantperson table */
UPDATE
    pp
SET
    pp.programcategoryid = x.programcategoryid
FROM
    eca_kmt_dev.eca_kmt_dev.dbo.participantperson pp
INNER JOIN
    dbo.dev_xref_Participantperson_update x 
ON
    x.eca_participantid = pp.participantid