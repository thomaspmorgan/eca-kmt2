  /* Updates field of study and position for participant */
UPDATE
    pp
SET
    pp.fieldofstudyid = x.fieldofstudyid,
	pp.positionid = x.positionid

FROM
    eca_dev.eca_dev.dbo.participantperson pp
INNER JOIN
    dbo.dev_xref_Participantperson_update x 
ON
    x.eca_participantid = pp.participantid