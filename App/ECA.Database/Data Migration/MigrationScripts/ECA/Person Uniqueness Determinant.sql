/* Used to determine which non-null columns define a unique Person/Participant */
SELECT last_name,first_name,birth_date,gender_cd,birth_city,birth_country,count(*)
FROM ce_person
WHERE birth_date IS NOT NULL AND birth_city IS NOT NULL AND birth_country IS NOT NULL AND gender_cd IS NOT NULL
GROUP BY last_name,first_name,birth_date,gender_cd,birth_city,birth_country
HAVING count(*) > 1
ORDER BY count(*) DESC