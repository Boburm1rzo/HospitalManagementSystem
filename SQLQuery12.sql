select d.FirstName as 'Doctor', s.Name
from Doctor d
inner join DoctorSpecialization ds on d.Id = ds.DoctorId
inner join Specialization s on s.Id = ds.SpecializationId
order by s.Name;

select d.FirstName as 'Doctor', COUNT(ds.Id) as 'Number of specializations'
from doctor d
inner join DoctorSpecialization ds on d.Id = ds.DoctorId
group by d.FirstName
order by d.FirstName;

select s.Name as 'Specialization', COUNT(ds.Id) as 'Number of Doctors'
from Specialization s
inner join DoctorSpecialization ds on s.Id = ds.SpecializationId
group by s.Name
order by s.Name;