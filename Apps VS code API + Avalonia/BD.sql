--
-- PostgreSQL database dump
--

\restrict md9PhUXemjyMqQdtK1W4XszsUGD32mOVbDLSFtaVeGhUfcNmln1rmE9Kv8CSzrv

-- Dumped from database version 17.6
-- Dumped by pg_dump version 17.6

-- Started on 2026-02-03 13:33:38

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 219 (class 1259 OID 18183)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 18173)
-- Name: employees; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.employees (
    employee_id integer NOT NULL,
    last_name character varying(50) NOT NULL,
    first_name character varying(50) NOT NULL,
    middle_name character varying(50),
    birth_date date,
    hire_date date,
    salary numeric(10,2),
    phone character varying(20),
    position_code integer,
    "PhotoPath" text
);


ALTER TABLE public.employees OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 18168)
-- Name: positions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.positions (
    position_code integer NOT NULL,
    position_name character varying(100) NOT NULL
);


ALTER TABLE public.positions OWNER TO postgres;

--
-- TOC entry 4903 (class 0 OID 18183)
-- Dependencies: 219
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20260130105139_AddEmployeePhoto	8.0.0
\.


--
-- TOC entry 4902 (class 0 OID 18173)
-- Dependencies: 218
-- Data for Name: employees; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.employees (employee_id, last_name, first_name, middle_name, birth_date, hire_date, salary, phone, position_code, "PhotoPath") FROM stdin;
10	Морозов	Степан	Владимирович	2008-07-08	\N	\N	\N	1	a69f619a-ad4d-4356-aad0-798b3711d25b.png
9	Беспалов	Кирилл		2012-01-04	\N	\N	\N	4	8f5b6d91-82fd-4b74-9d4e-b707be9ef81c.png
5	Чижикова	Марина	Петровна	2005-07-11	2014-03-12	100000.00	89334445566	2	080e54a2-039a-4484-bb1d-1773d344ebf6.png
11	Морозова	Лариса	Николаевна	1981-09-05	\N	\N	\N	5	ea1e7a4b-0954-411a-9201-9a51aa2439b0.png
4	Кузнецов	Дмитрий	Андреевич	1988-01-30	2017-05-20	85000.00	89223334455	3	c517a13c-326c-4aa0-848f-0fcac5e1370f.png
3	Сидорова	Марина	Викторовна	1992-07-18	2019-02-15	95000.00	89112223344	3	ff3f0e36-8495-4692-91d0-09a747295107.png
\.


--
-- TOC entry 4901 (class 0 OID 18168)
-- Dependencies: 217
-- Data for Name: positions; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.positions (position_code, position_name) FROM stdin;
1	Директор
2	Бухгалтер
3	Менеджер
4	Программист
5	Системный администратор
\.


--
-- TOC entry 4754 (class 2606 OID 18187)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 4752 (class 2606 OID 18177)
-- Name: employees employees_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.employees
    ADD CONSTRAINT employees_pkey PRIMARY KEY (employee_id);


--
-- TOC entry 4750 (class 2606 OID 18172)
-- Name: positions positions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.positions
    ADD CONSTRAINT positions_pkey PRIMARY KEY (position_code);


--
-- TOC entry 4755 (class 2606 OID 18178)
-- Name: employees employees_position_code_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.employees
    ADD CONSTRAINT employees_position_code_fkey FOREIGN KEY (position_code) REFERENCES public.positions(position_code);


-- Completed on 2026-02-03 13:33:38

--
-- PostgreSQL database dump complete
--

\unrestrict md9PhUXemjyMqQdtK1W4XszsUGD32mOVbDLSFtaVeGhUfcNmln1rmE9Kv8CSzrv

