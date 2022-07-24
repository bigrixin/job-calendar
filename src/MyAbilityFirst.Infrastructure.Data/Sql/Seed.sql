-- ***************************************************************************************************************************
-- Seed Data
-- ***************************************************************************************************************************

declare @categoryname as varchar(255) 
declare @categoryID as int

set @categoryname = 'Gender'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Male')
		insert into Subcategory values (@categoryid,'Female')
		insert into Subcategory values (@categoryid,'Other')
		insert into Subcategory values (@categoryid,'None / Not Disclosed')

set @categoryname = 'ClientMarketingInfo'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Existing Client')
		insert into Subcategory values (@categoryid,'Previous Client')
		insert into Subcategory values (@categoryid,'Search Engine')
		insert into Subcategory values (@categoryid,'Social Media')
		insert into Subcategory values (@categoryid,'Forum or Blog')
		insert into Subcategory values (@categoryid,'TV')
		insert into Subcategory values (@categoryid,'Newspaper')
		insert into Subcategory values (@categoryid,'Magazine')
		insert into Subcategory values (@categoryid,'Radio')
		insert into Subcategory values (@categoryid,'Friend / Family')
		insert into Subcategory values (@categoryid,'Event')
		insert into Subcategory values (@categoryid,'Flyer or Card')
		insert into Subcategory values (@categoryid,'Business / Supplier')
		insert into Subcategory values (@categoryid,'Case Manager')
		insert into Subcategory values (@categoryid,'Hospital Staff')
		insert into Subcategory values (@categoryid,'NDIS')
		insert into Subcategory values (@categoryid,'School')
		insert into Subcategory values (@categoryid,'Doctor / Medical Practitioner')
		insert into Subcategory values (@categoryid,'Care / Support Worker')
		insert into Subcategory values (@categoryid,'Other')

set @categoryname = 'CareType'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Disability Support')
		insert into Subcategory values (@categoryid,'Aged Care')
		insert into Subcategory values (@categoryid,'Mental Health')
		insert into Subcategory values (@categoryid,'Post-Surgery')
		insert into Subcategory values (@categoryid,'Other')

set @categoryname = 'CareWorkerMarketingInfo'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Existing Care Worker')
		insert into Subcategory values (@categoryid,'Previous Care Worker')
		insert into Subcategory values (@categoryid,'Search Engine')
		insert into Subcategory values (@categoryid,'Social Media')
		insert into Subcategory values (@categoryid,'Forum or Blog')
		insert into Subcategory values (@categoryid,'TV')
		insert into Subcategory values (@categoryid,'Newspaper')
		insert into Subcategory values (@categoryid,'Magazine')
		insert into Subcategory values (@categoryid,'Radio')
		insert into Subcategory values (@categoryid,'Friend / Family')
		insert into Subcategory values (@categoryid,'Event')
		insert into Subcategory values (@categoryid,'Flyer or Card')
		insert into Subcategory values (@categoryid,'Seek')
		insert into Subcategory values (@categoryid,'LinkedIn')
		insert into Subcategory values (@categoryid,'CareerOne')
		insert into Subcategory values (@categoryid,'Blue Steps')
		insert into Subcategory values (@categoryid,'Gumtree')
		insert into Subcategory values (@categoryid,'Indeed')
		insert into Subcategory values (@categoryid,'Other Job Website')
		insert into Subcategory values (@categoryid,'Colleague')
		insert into Subcategory values (@categoryid,'Teacher')
		insert into Subcategory values (@categoryid,'Careers Advisor')
		insert into Subcategory values (@categoryid,'Other')

set @categoryname = 'QualificationType'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Nursing Services')
		insert into Subcategory values (@categoryid,'Personal Care')
		insert into Subcategory values (@categoryid,'Social & Domestic Assistance')

set @categoryname = 'NursingQualifications'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Enrolled Nurse')
		insert into Subcategory values (@categoryid,'Registered Nurse')
		insert into Subcategory values (@categoryid,'One or more years of nursing experience')
		insert into Subcategory values (@categoryid,'Registeration number')

set @categoryname = 'PersonalCareQualifications'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Certificate 3 Individual Support')
		insert into Subcategory values (@categoryid,'Certificate 4 Individual support')
		insert into Subcategory values (@categoryid,'Certificate 3 Aged Care')
		insert into Subcategory values (@categoryid,'Certificate 4 Aged Care')
		insert into Subcategory values (@categoryid,'Certificate 3 in Disabilities')
		insert into Subcategory values (@categoryid,'Certificate 4 in Disabilities')
		insert into Subcategory values (@categoryid,'Degree in Nursing')
		insert into Subcategory values (@categoryid,'Degree in allied Health')
		insert into Subcategory values (@categoryid,'Certificate 3 in Home and Community Care')
		insert into Subcategory values (@categoryid,'Certificate 4 in Home and Community Care')
		insert into Subcategory values (@categoryid,'Assistant in Nursing (AIN)')
		insert into Subcategory values (@categoryid,'Other Relevant Qualification')
		insert into Subcategory values (@categoryid,'Working Toward Degree in Nursing')
		insert into Subcategory values (@categoryid,'2 or more Years of Experience')
		insert into Subcategory values (@categoryid,'5 or more Years of Experience')
		insert into Subcategory values (@categoryid,'10 or more Years of Experience')

set @categoryname = 'OtherQualifications'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Current First Aid (Level 1)')
		insert into Subcategory values (@categoryid,'Current Senior First Aid (Level 2)')
		insert into Subcategory values (@categoryid,'Palliative Care')
		insert into Subcategory values (@categoryid,'Manual Handling Certified')
		insert into Subcategory values (@categoryid,'Dementia Care')
		insert into Subcategory values (@categoryid,'Medication Assistance')
		insert into Subcategory values (@categoryid,'Food Handling')
		insert into Subcategory values (@categoryid,'LGBTI Health alliance Inclusive Training')
		insert into Subcategory values (@categoryid,'Hoist')
		insert into Subcategory values (@categoryid,'PEG Feeding')
		insert into Subcategory values (@categoryid,'Bowel and Bladder Management')
		insert into Subcategory values (@categoryid,'Water Safety')
		insert into Subcategory values (@categoryid,'Certificate 3 Community Services')
		insert into Subcategory values (@categoryid,'Certificate 4 Community Services')
		insert into Subcategory values (@categoryid,'Degree in Psychology')
		insert into Subcategory values (@categoryid,'Degree in Social Work')
		insert into Subcategory values (@categoryid,'Valid Driver’s License')
		insert into Subcategory values (@categoryid,'Registered Vehicle')


set @categoryname = 'VerificationChecks'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Passport')
		insert into Subcategory values (@categoryid,'National Police Criminal Records Check')
		insert into Subcategory values (@categoryid,'Working with Children Check (WWCC)')
		insert into Subcategory values (@categoryid,'Working with Vulnerable People Check (WWVPC)')



set @categoryname = 'FormalEducation'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Completed Year 9-11')
		insert into Subcategory values (@categoryid,'Completed Year 12')
		insert into Subcategory values (@categoryid,'Diploma')
		insert into Subcategory values (@categoryid,'TAFE/Trade Certificate')
		insert into Subcategory values (@categoryid,'University Undergraduate')
		insert into Subcategory values (@categoryid,'Post Graduate Degree')
		insert into Subcategory values (@categoryid,'Masters')
		insert into Subcategory values (@categoryid,'PhD')




set @categoryname = 'ServiceSubType'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Activities, Outings and Community Access')
		insert into Subcategory values (@categoryid,'Assistance with Eating')
		insert into Subcategory values (@categoryid,'Assistance with Ventilator')
		insert into Subcategory values (@categoryid,'Assist with Bowel and Bladder Management')
		insert into Subcategory values (@categoryid,'Assist with Self Medication')
		insert into Subcategory values (@categoryid,'Bowel and Bladder Management')
		insert into Subcategory values (@categoryid,'Care Assessment, Planning, Co-ordination')
		insert into Subcategory values (@categoryid,'Case Assessment and Management')
		insert into Subcategory values (@categoryid,'Catheter Care')
		insert into Subcategory values (@categoryid,'Cleaning and Laundry')
		insert into Subcategory values (@categoryid,'Companionship')
		insert into Subcategory values (@categoryid,'Continence Assessment and Management')
		insert into Subcategory values (@categoryid,'Exercise Assistance')
		insert into Subcategory values (@categoryid,'Hoist and Transfer')
		insert into Subcategory values (@categoryid,'Home Maintenance')
		insert into Subcategory values (@categoryid,'Life skills Development')
		insert into Subcategory values (@categoryid,'Lifestyle Co-ordinator')
		insert into Subcategory values (@categoryid,'Light Gardening')
		insert into Subcategory values (@categoryid,'Light Housework')
		insert into Subcategory values (@categoryid,'Light Massage')
		insert into Subcategory values (@categoryid,'Manual Handling (Getting in and out of Bed)')
		insert into Subcategory values (@categoryid,'Meal Preparation')
		insert into Subcategory values (@categoryid,'Medication Management')
		insert into Subcategory values (@categoryid,'Nursing Services')
		insert into Subcategory values (@categoryid,'Palliative Care')
		insert into Subcategory values (@categoryid,'Palliative Nursing Care')
		insert into Subcategory values (@categoryid,'PEG Feeding')
		insert into Subcategory values (@categoryid,'Personal Assistant (Administration)')
		insert into Subcategory values (@categoryid,'Pre and Post-Acute Hospital Care')
		insert into Subcategory values (@categoryid,'Respiratory Care')
		insert into Subcategory values (@categoryid,'Shopping')
		insert into Subcategory values (@categoryid,'Showering, Dressing, Grooming')
		insert into Subcategory values (@categoryid,'Sports and Exercise')
		insert into Subcategory values (@categoryid,'Toileting')
		insert into Subcategory values (@categoryid,'Transport')
		insert into Subcategory values (@categoryid,'Vital Signs Monitoring')
		insert into Subcategory values (@categoryid,'Wound Care')

set @categoryname = 'OtherPreference'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'LGBTI Friendly')
		insert into Subcategory values (@categoryid,'Pet Friendly')
		insert into Subcategory values (@categoryid,'Non-Smoker')

set @categoryname = 'DisabilityCareExperience'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Acquired Brain Injury')
		insert into Subcategory values (@categoryid,'Autism')
		insert into Subcategory values (@categoryid,'Cerebral Palsy')
		insert into Subcategory values (@categoryid,'Cystic Fibrosis')
		insert into Subcategory values (@categoryid,'Down Syndrome')
		insert into Subcategory values (@categoryid,'Epilepsy')
		insert into Subcategory values (@categoryid,'Hearing Impairment')
		insert into Subcategory values (@categoryid,'Intellectual Disabilities')
		insert into Subcategory values (@categoryid,'Motor Neuron Disease')
		insert into Subcategory values (@categoryid,'Muscular Dystrophy')
		insert into Subcategory values (@categoryid,'Physical Disabilities')
		insert into Subcategory values (@categoryid,'Spina Bifida')
		insert into Subcategory values (@categoryid,'Spinal Cord Injury')
		insert into Subcategory values (@categoryid,'Vision Impairment')

set @categoryname = 'MentalHealthCareExperience'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Anxiety')
		insert into Subcategory values (@categoryid,'Bipolar Disorder')
		insert into Subcategory values (@categoryid,'Depression')
		insert into Subcategory values (@categoryid,'Eating Disorders')
		insert into Subcategory values (@categoryid,'Hoarding')
		insert into Subcategory values (@categoryid,'Obsessive Compulsive Disorder (OCD)')
		insert into Subcategory values (@categoryid,'Post-Traumatic Stress Disorder (PTSD)')
		insert into Subcategory values (@categoryid,'Schizophrenia')
		insert into Subcategory values (@categoryid,'Substance Abuse and Addiction')

set @categoryname = 'AgedCareExperience'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Dementia')
		insert into Subcategory values (@categoryid,'Parkinson’s Disease')

set @categoryname = 'ChronicMedicalConditionsCareExperience'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Arthritis')
		insert into Subcategory values (@categoryid,'Asthma')
		insert into Subcategory values (@categoryid,'Cardiovascular Disease')
		insert into Subcategory values (@categoryid,'COPD or Respiratory Illness')
		insert into Subcategory values (@categoryid,'Diabetes')

set @categoryname = 'Personality'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Extroverted, outgoing and energetic')
		insert into Subcategory values (@categoryid,'Calm and relaxed')
		insert into Subcategory values (@categoryid,'Introverted, quiet and reserved')

set @categoryname = 'Relationship'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Partner / Spouse')
		insert into Subcategory values (@categoryid,'Immediate Family')
		insert into Subcategory values (@categoryid,'Extended Family')
		insert into Subcategory values (@categoryid,'Legal Representative/ Advocate')
		insert into Subcategory values (@categoryid,'Approved Provider')
		insert into Subcategory values (@categoryid,'Care Co-ordinator')
		insert into Subcategory values (@categoryid,'Friend / Neighbour')

set @categoryname = 'Interest'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Cooking')
		insert into Subcategory values (@categoryid,'Gardening')
		insert into Subcategory values (@categoryid,'Indoor Games / Puzzles')
		insert into Subcategory values (@categoryid,'Movies')
		insert into Subcategory values (@categoryid,'Music')
		insert into Subcategory values (@categoryid,'Cultural Festivals')
		insert into Subcategory values (@categoryid,'Pets')
		insert into Subcategory values (@categoryid,'Photography / Art')
		insert into Subcategory values (@categoryid,'Reading')
		insert into Subcategory values (@categoryid,'Sports')
		insert into Subcategory values (@categoryid,'Travel')
		insert into Subcategory values (@categoryid,'Working')
		insert into Subcategory values (@categoryid,'Other')

set @categoryname = 'Language'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Arabic')
		insert into Subcategory values (@categoryid,'Cantonese')
		insert into Subcategory values (@categoryid,'Croatian')
		insert into Subcategory values (@categoryid,'English')
		insert into Subcategory values (@categoryid,'French')
		insert into Subcategory values (@categoryid,'German')
		insert into Subcategory values (@categoryid,'Greek')
		insert into Subcategory values (@categoryid,'Hebrew')
		insert into Subcategory values (@categoryid,'Hindi')
		insert into Subcategory values (@categoryid,'Hungarian')
		insert into Subcategory values (@categoryid,'Indonesian')
		insert into Subcategory values (@categoryid,'Italian')
		insert into Subcategory values (@categoryid,'Japanese')
		insert into Subcategory values (@categoryid,'Korean')
		insert into Subcategory values (@categoryid,'Mandarin')
		insert into Subcategory values (@categoryid,'Maltese')
		insert into Subcategory values (@categoryid,'Macedonian')
		insert into Subcategory values (@categoryid,'Netherlandic ')
		insert into Subcategory values (@categoryid,'Persian')
		insert into Subcategory values (@categoryid,'Polish')
		insert into Subcategory values (@categoryid,'Portugese ')
		insert into Subcategory values (@categoryid,'Russian')
		insert into Subcategory values (@categoryid,'Serbian')
		insert into Subcategory values (@categoryid,'Sinhalese')
		insert into Subcategory values (@categoryid,'Samoan ')
		insert into Subcategory values (@categoryid,'Spanish')
		insert into Subcategory values (@categoryid,'Tamil')
		insert into Subcategory values (@categoryid,'Tagalog (Filipino)')
		insert into Subcategory values (@categoryid,'Turkish')
		insert into Subcategory values (@categoryid,'Vietnamese')
		insert into Subcategory values (@categoryid,'Auslan (Australian sign Language)')
		insert into Subcategory values (@categoryid,'Other')

set @categoryname = 'Culture'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Australian')
		insert into Subcategory values (@categoryid,'Australian Aboriginal')
		insert into Subcategory values (@categoryid,'Australian South East Islander')
		insert into Subcategory values (@categoryid,'Torres Strait Islander')
		insert into Subcategory values (@categoryid,'New Zealander')
		insert into Subcategory values (@categoryid,'Maori')
		insert into Subcategory values (@categoryid,'Polynesian')
		insert into Subcategory values (@categoryid,'Other Oceanian')
		insert into Subcategory values (@categoryid,'Western European')
		insert into Subcategory values (@categoryid,'Northern European')
		insert into Subcategory values (@categoryid,'Southern & Eastern European')
		insert into Subcategory values (@categoryid,'Middle Eastern')
		insert into Subcategory values (@categoryid,'Jewish')
		insert into Subcategory values (@categoryid,'Asian')
		insert into Subcategory values (@categoryid,'North American')
		insert into Subcategory values (@categoryid,'South American')
		insert into Subcategory values (@categoryid,'Central American')
		insert into Subcategory values (@categoryid,'Caribbean Islander')
		insert into Subcategory values (@categoryid,'South African')
		insert into Subcategory values (@categoryid,'Other African')
		insert into Subcategory values (@categoryid,'Other')

set @categoryname = 'Religion'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Anglican')
		insert into Subcategory values (@categoryid,'Buddhist')
		insert into Subcategory values (@categoryid,'Catholic')
		insert into Subcategory values (@categoryid,'Christian')
		insert into Subcategory values (@categoryid,'Greek Orthodox')
		insert into Subcategory values (@categoryid,'Hindu')
		insert into Subcategory values (@categoryid,'Islamic')
		insert into Subcategory values (@categoryid,'Jewish')
		insert into Subcategory values (@categoryid,'Presbyterian')
		insert into Subcategory values (@categoryid,'Russian Orthodox')
		insert into Subcategory values (@categoryid,'Sikh')
		insert into Subcategory values (@categoryid,'Spiritual')
		insert into Subcategory values (@categoryid,'Uniting Church ')
		insert into Subcategory values (@categoryid,'Other')
		insert into Subcategory values (@categoryid,'None')

set @categoryname = 'Pet'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Dog')
		insert into Subcategory values (@categoryid,'Cat')
		insert into Subcategory values (@categoryid,'Other')

set @categoryname = 'MedicalLivingNeed'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Cardiac / Vascular')
		insert into Subcategory values (@categoryid,'Diabetes / Cholesterol')
		insert into Subcategory values (@categoryid,'Incontinence (Bladder or Bowel)')
		insert into Subcategory values (@categoryid,'Memory Loss / Dementia')
		insert into Subcategory values (@categoryid,'Nutrition / Hydration')
		insert into Subcategory values (@categoryid,'Other Barriers or Concerns')
		insert into Subcategory values (@categoryid,'Physical / Mobility')
		insert into Subcategory values (@categoryid,'Complex communication ')
		insert into Subcategory values (@categoryid,'Psychological / Behavioral ')
		insert into Subcategory values (@categoryid,'Skin Integrity / Wound Management')
		insert into Subcategory values (@categoryid,'Speech / Swallowing')

set @categoryname = 'JobService'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
		insert into Subcategory values (@categoryid,'Companionship & Social Support')
		insert into Subcategory values (@categoryid,'Cleaning & Laundry')
		insert into Subcategory values (@categoryid,'Community Participation, Sports & Activities')
		insert into Subcategory values (@categoryid,'Showering, Toileting & Dressing')
		insert into Subcategory values (@categoryid,'Transport')
		insert into Subcategory values (@categoryid,'Manual Transfer & Mobility')
		insert into Subcategory values (@categoryid,'Independent Living Skills')
		insert into Subcategory values (@categoryid,'Assist with Self Medication')
		insert into Subcategory values (@categoryid,'Meal Preparation & Shopping ')
		insert into Subcategory values (@categoryid,'Nursing Services')

set @categoryname = 'Attachment'
insert into Category values (@categoryname)
	select @categoryID = ID from Category where Name = @categoryname
	 insert into SubCategory values (@categoryid,'Photo')
	 insert into SubCategory values (@categoryid,'Care Plan')
	 insert into SubCategory values (@categoryid,'NDIS approved Plan')
	 insert into SubCategory values (@categoryid,'GP Documents')
 	 insert into SubCategory values (@categoryid,'Birth Certificate')
	 insert into SubCategory values (@categoryid,'Medicare')
	 insert into SubCategory values (@categoryid,'Proof of Age')
	 insert into SubCategory values (@categoryid,'Psychology Report')
	 insert into SubCategory values (@categoryid,'Review and Assessment Reports')

	 -- for organisation --
insert into Organisation (Name, LogoURL, Address, Phone, Introduction )
	values ('CPL','Images/CPL.jpg','','','')
insert into Organisation (Name, LogoURL, Address, Phone, Introduction )
	values ('Cootharinga','Images/Cootharinga.jpg','','','')
insert into Organisation (Name, LogoURL, Address, Phone, Introduction )
	values ('Multicap','Images/Multicap.jpg','','','')

