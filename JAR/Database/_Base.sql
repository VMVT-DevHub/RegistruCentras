CREATE SCHEMA jar AUTHORIZATION _master_admin;
GRANT ALL ON SCHEMA jar TO _master_admin WITH GRANT OPTION;
GRANT USAGE ON SCHEMA jar TO repl_jar;
SET SESSION AUTHORIZATION "_master_admin";



CREATE TABLE jar.raw_rc_jar (
	jar_id bigint not null,
	jar_kodas bigint not null,
	jar_pavadinimas varchar(255),
	jar_pastabos varchar,
	jar_komentaras varchar,
	jar_prist_dez_adr bigint,
	jar_forma int,
	jar_ntr_kodas bigint,
	jar_dbuk_kodas int,
	jar_otip_kodas int,
	jar_reje_kodas int,
	jar_data_reg date,
	jar_data_isreg date,
	jar_data_steigimo date,
	jar_anul int,
	jar_tikslai varchar,
	jar_pagrindinis int,
	jar_versija int,
	jar_versijos_data date,
	jar_pagr_id int,
	jar_pagr_kodas bigint,
	jar_pagr_pavadinimas varchar(255)
);


CREATE TABLE jar.raw_rc_ja_formos (
	jfm_jar_id bigint not null,
	jfm_kodas int not null,
	jfm_data_nuo date,
	jfm_data_iki date,
	jfm_anul int
);
CREATE TABLE jar.raw_rc_ja_atributai (
	jat_jar_id bigint not null,
	jat_kodas int not null,
	jat_reiksme varchar(255),
	jat_data_nuo timestamp(0),
	jat_data_iki date,
	jat_vien_kodas int
);
CREATE TABLE jar.raw_rc_ja_statusai (
	jst_jar_id bigint not null,
	jst_status int not null,
	jst_data_igijimo date,
	jst_data_netekimo date,
	jst_data_nuo date,
	jst_data_iki date,
	jst_ispr_kodas int,
	jst_anul int
);
CREATE TABLE jar.raw_rc_ja_veiklos (
	jvk_jar_id bigint not null,
	jvk_kodas int not null,
	jvk_versija int,
	jvk_data_nuo timestamp(0),
	jvk_data_iki date
);
CREATE TABLE jar.raw_rc_ja_pavadinimai (
	jpv_jar_id bigint not null,
	jpv_kodas int not null,
	jpv_reg_nr varchar(255),
	jpv_data_reg date,
	jpv_data_nuo timestamp(0),
	jpv_data_iki date,
	jpv_pavadinimas varchar(255),
	jpv_anul int
);
CREATE TABLE jar.raw_rc_ja_adresai (
	jad_jar_id bigint not null,
	jad_aob bigint,
	jad_pavadinimas varchar(255),
	jad_data_nuo timestamp(0),
	jad_data_iki date,
	jad_adr_bus int
);
CREATE TABLE jar.raw_rc_ja_tekstai (
	jtx_jar_id bigint not null,
	jtx_kodas int not null,
	jtx_tekstas text,
	jtx_data_nuo timestamp(0),
	jtx_data_iki date
);
CREATE TABLE jar.raw_rc_ja_nt_objektai (
	jnt_jar_id bigint not null,
	jnt_ntr bigint not null,
	jnt_data_nuo timestamp(0),
	jnt_data_iki date
);
CREATE TABLE jar.raw_rc_ja_asmenys (
	jas_jar_id bigint not null,
	jas_id bigint not null,
	jas_fa_id bigint,
	jas_ja_id bigint,
	jas_neid_id bigint,
	jas_nja_id bigint,
	jas_ste_id int,
	jas_ntr_id bigint,
	jar_ntr_kodas varchar(255)
);

CREATE TABLE jar.raw_rc_ja_finansine_atskaitomybe (
	jfa_jar_id bigint not null,
	jfa_data_nuo date,
	jfa_data_iki date,
	jfa_data_pateik date
);
CREATE TABLE jar.raw_rc_ja_filialai (
	jfl_jar_id bigint not null,
	jfl_kodas bigint not null,
	jfl_pavadinimas varchar(255),
	jfl_data_reg date,
	jfl_data_steig date
);


CREATE TABLE jar.raw_rc_ja_faktai (
	jfk_jar_id bigint not null,
	jfk_aprasymas text,
	jfk_data_pradzios date,
	jfk_data_pabaigos date,
	jfk_data_term_nuo date,
	jfk_data_term_iki date,
	jfk_tipas int,
	jfk_potipis int,
	jfk_anul int,
	jfk_kvo int,
	jfk_katId int,
	jfk_prokur_nr bigint,
	jfk_dokumentai bigint[],
	jfk_naudotojai jsonb,
	jfk_kategorijos jsonb
);

CREATE TABLE jar.raw_rc_dokumentai (
	dok_id bigint not null,
	dok_kodas bigint,
	dok_nr varchar(255),
	dok_pastabos varchar(255),
	dok_data date,
	dok_data_pateik date,
	dok_data_reg date,
	dok_tipas int not null,
	dok_potipis int not null,
	dok_notaro_nr varchar(255),
	dok_fiz_id int,
	dok_anul int
);

CREATE TABLE jar.raw_rc_fiziniai_asmenys (
	fiz_id bigint not null,
	fiz_kodas bigint,
	fiz_salis int,
	fiz_vardas varchar(255),
	fiz_pavarde varchar(255),
	fiz_data_gimimo date,
	fiz_data_mirties date,
	fiz_uzsien_kodas varchar(255),
	fiz_adr_bus int,
	fiz_anul int
);

CREATE TABLE jar.raw_rc_fizasm_adresas (
	adr_fiz_id bigint not null,
	adr_aob bigint,
	adr_adresas varchar(255),
	adr_salis int,
	adr_bus int
);

CREATE TABLE jar.raw_rc_fizasm_neid (
	ned_id bigint not null,
	ned_vardas varchar(255),
	ned_pavarde varchar(255),
	ned_pastabos varchar,
	ned_data_gimimo date,
	ned_data_mirties date,
	ned_aob bigint,
	ned_adresas varchar(255),
	ned_adr_salis int,
	ned_adr_bus int
);

CREATE TABLE jar.raw_rc_uszien_steigejai (
	ust_id bigint not null,
	ust_kodas varchar(255),
	ust_pavadinimas varchar(255),
	ust_pastaba varchar,
	ust_veikla varchar,
	ust_data_reg date,
	ust_teis_forma varchar(255),
	ust_teis_status varchar(255),
	ust_adresas varchar(255),
	ust_registras varchar(255),
	ust_salis int,
	ust_sal_kodas varchar(255),
	ust_kapitalas bigint,
	usr_f_metai_nuo varchar(255)
);

CREATE TABLE jar.raw_rc_uszien_statusai (
	uzs_ust_id bigint not null,
	uzs_status varchar(255),
	uzs_data date,
	uzs_data_nuo date,
	uzs_data_iki date
);

CREATE TABLE jar.raw_rc_uszien_pavadinimai (
	uzp_ust_id bigint not null,
	uzp_pavadinimas varchar(255),
	uzp_data_nuo date,
	uzp_data_iki date
);

CREATE TABLE jar.raw_rc_uszien_adresai (
	uza_ust_id bigint not null,
	uza_adresas varchar(255),
	uza_data_nuo date,
	uza_data_iki date
);

CREATE TABLE jar.raw_rc_klasifikatoriai (
	clf_grupe varchar(255),
	clf_kodas integer,
	clf_tipas integer,
	clf_potipis integer,
	clf_reiksme varchar(255),
	clf_pavadinimas varchar,
	clf_pavadinimas_en varchar,
	clf_data_nuo timestamp(0),
	clf_data_iki timestamp(0)	
);

CREATE TABLE jar.log_updates (
    log_id bigint NOT NULL GENERATED ALWAYS AS IDENTITY,
    log_date timestamp(3) DEFAULT timezone('utc'::text, now()),
    log_jar integer,
    CONSTRAINT jar_log_updates PRIMARY KEY (log_id)
);


CREATE INDEX idx_raw_rc_jar_id ON jar.raw_rc_jar (jar_id);
CREATE INDEX idx_raw_rc_jar_kodas ON jar.raw_rc_jar (jar_kodas);
CREATE INDEX idx_log_updates_jar ON jar.log_updates (log_jar);

ALTER TABLE jar.raw_rc_jar ADD CONSTRAINT raw_rc_jar_pkey PRIMARY KEY (jar_id);
ALTER TABLE jar.raw_rc_jar ADD CONSTRAINT raw_rc_jar_unique UNIQUE (jar_kodas);

/*
CREATE OR REPLACE VIEW jar.v_raw_iregistruoti AS
   SELECT j.ja_kodas,COALESCE(r.jar_pavadinimas,j.ja_pavadinimas) as ja_pavadinimas,j.adresas,a.aob_kodas,form_kodas,form_pavadinimas,status_kodas, COALESCE(s.stat_pavad,stat_pavadinimas) stat_pavadinimas,stat_data,reg_data,a.adresas_nuo aob_data,j.formavimo_data 
   FROM jar.raw_iregistruoti j LEFT JOIN jar.raw_rc_jar r on (j.ja_kodas=r.jar_kodas) LEFT JOIN jar.clf_status s on (j.status_kodas=s.stat_id) LEFT JOIN jar.raw_adresai a on (j.ja_kodas=a.ja_kodas);
*/

