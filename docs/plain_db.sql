--
-- PostgreSQL database dump
--

-- Dumped from database version 14.10
-- Dumped by pg_dump version 15.3

-- Started on 2024-01-01 18:01:16

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 217 (class 1259 OID 114709)
-- Name: rls_inventory_items; Type: TABLE; Schema: public; Owner: hoangduy06104
--

CREATE TABLE public.rls_inventory_items (
    id integer NOT NULL,
    inventory_plan_id integer NOT NULL,
    item_id integer NOT NULL,
    note text,
    status smallint NOT NULL
);

--
-- TOC entry 218 (class 1259 OID 114726)
-- Name: rls_inventory_items_id_seq; Type: SEQUENCE; Schema: public; Owner: hoangduy06104
--

ALTER TABLE public.rls_inventory_items ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.rls_inventory_items_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 213 (class 1259 OID 106496)
-- Name: rls_rental_items; Type: TABLE; Schema: public; Owner: hoangduy06104
--

CREATE TABLE public.rls_rental_items (
    id integer NOT NULL,
    user_id integer NOT NULL,
    item_id integer NOT NULL,
    rental_start timestamp with time zone DEFAULT now() NOT NULL,
    expect_return timestamp with time zone,
    actual_return timestamp with time zone,
    status smallint DEFAULT 0 NOT NULL
);

--
-- TOC entry 214 (class 1259 OID 106502)
-- Name: rls_rental_items_id_seq; Type: SEQUENCE; Schema: public; Owner: hoangduy06104
--

ALTER TABLE public.rls_rental_items ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.rls_rental_items_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 215 (class 1259 OID 114694)
-- Name: tbl_inventory_plans; Type: TABLE; Schema: public; Owner: hoangduy06104
--

CREATE TABLE public.tbl_inventory_plans (
    id integer NOT NULL,
    name character varying NOT NULL,
    note text,
    assignee_id integer DEFAULT 0 NOT NULL,
    status smallint DEFAULT 0 NOT NULL,
    deadline timestamp with time zone DEFAULT now() NOT NULL
);

--
-- TOC entry 216 (class 1259 OID 114708)
-- Name: tbl_inventory_plans_id_seq; Type: SEQUENCE; Schema: public; Owner: hoangduy06104
--

ALTER TABLE public.tbl_inventory_plans ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.tbl_inventory_plans_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 211 (class 1259 OID 98304)
-- Name: tbl_items; Type: TABLE; Schema: public; Owner: hoangduy06104
--

CREATE TABLE public.tbl_items (
    id integer NOT NULL,
    name character varying NOT NULL,
    status smallint DEFAULT 0,
    room character varying,
    description character varying,
    note character varying
);


--
-- TOC entry 212 (class 1259 OID 98312)
-- Name: tbl_items_id_seq; Type: SEQUENCE; Schema: public; Owner: hoangduy06104
--

ALTER TABLE public.tbl_items ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.tbl_items_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 209 (class 1259 OID 81920)
-- Name: tbl_users; Type: TABLE; Schema: public; Owner: hoangduy06104
--

CREATE TABLE public.tbl_users (
    id integer NOT NULL,
    username character varying(255) NOT NULL,
    password character varying(255) NOT NULL,
    name character varying(255),
    email character varying(255),
    phone character varying(255),
    address character varying(255),
    role smallint DEFAULT 4 NOT NULL
);


--
-- TOC entry 210 (class 1259 OID 81927)
-- Name: tbl_users_id_seq; Type: SEQUENCE; Schema: public; Owner: hoangduy06104
--

ALTER TABLE public.tbl_users ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.tbl_users_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 2565 (class 0 OID 81920)
-- Dependencies: 209
-- Data for Name: tbl_users; Type: TABLE DATA; Schema: public; Owner: hoangduy06104
--

INSERT INTO public.tbl_users (username, password, name, email, phone, address, role)
VALUES ('admin', '21232F297A57A5A743894A0E4A801FC3', 'ROOT', 'admin@localhost.com', '0900000000', 'TP.HCM', 1);


--
-- TOC entry 2420 (class 2606 OID 122881)
-- Name: rls_inventory_items pkey; Type: CONSTRAINT; Schema: public; Owner: hoangduy06104
--

ALTER TABLE ONLY public.rls_inventory_items
    ADD CONSTRAINT pkey PRIMARY KEY (inventory_plan_id, item_id);


--
-- TOC entry 2416 (class 2606 OID 106501)
-- Name: rls_rental_items rls_rental_items_pkey; Type: CONSTRAINT; Schema: public; Owner: hoangduy06104
--

ALTER TABLE ONLY public.rls_rental_items
    ADD CONSTRAINT rls_rental_items_pkey PRIMARY KEY (id);


--
-- TOC entry 2418 (class 2606 OID 114702)
-- Name: tbl_inventory_plans tbl_inventory_plans_pkey; Type: CONSTRAINT; Schema: public; Owner: hoangduy06104
--

ALTER TABLE ONLY public.tbl_inventory_plans
    ADD CONSTRAINT tbl_inventory_plans_pkey PRIMARY KEY (id);


--
-- TOC entry 2412 (class 2606 OID 98311)
-- Name: tbl_items tbl_items_pkey; Type: CONSTRAINT; Schema: public; Owner: hoangduy06104
--

ALTER TABLE ONLY public.tbl_items
    ADD CONSTRAINT tbl_items_pkey PRIMARY KEY (id);


--
-- TOC entry 2410 (class 2606 OID 81926)
-- Name: tbl_users tbl_users_pkey; Type: CONSTRAINT; Schema: public; Owner: hoangduy06104
--

ALTER TABLE ONLY public.tbl_users
    ADD CONSTRAINT tbl_users_pkey PRIMARY KEY (id);


--
-- TOC entry 2413 (class 1259 OID 114738)
-- Name: fki_item; Type: INDEX; Schema: public; Owner: hoangduy06104
--

CREATE INDEX fki_item ON public.rls_rental_items USING btree (item_id);


--
-- TOC entry 2414 (class 1259 OID 114732)
-- Name: fki_user; Type: INDEX; Schema: public; Owner: hoangduy06104
--

CREATE INDEX fki_user ON public.rls_rental_items USING btree (user_id);


--
-- TOC entry 2423 (class 2606 OID 114703)
-- Name: tbl_inventory_plans assignee_id; Type: FK CONSTRAINT; Schema: public; Owner: hoangduy06104
--

ALTER TABLE ONLY public.tbl_inventory_plans
    ADD CONSTRAINT assignee_id FOREIGN KEY (assignee_id) REFERENCES public.tbl_users(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2424 (class 2606 OID 114716)
-- Name: rls_inventory_items inventory_plan; Type: FK CONSTRAINT; Schema: public; Owner: hoangduy06104
--

ALTER TABLE ONLY public.rls_inventory_items
    ADD CONSTRAINT inventory_plan FOREIGN KEY (inventory_plan_id) REFERENCES public.tbl_inventory_plans(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2421 (class 2606 OID 114733)
-- Name: rls_rental_items item; Type: FK CONSTRAINT; Schema: public; Owner: hoangduy06104
--

ALTER TABLE ONLY public.rls_rental_items
    ADD CONSTRAINT item FOREIGN KEY (item_id) REFERENCES public.tbl_items(id) ON UPDATE CASCADE ON DELETE CASCADE NOT VALID;


--
-- TOC entry 2425 (class 2606 OID 114721)
-- Name: rls_inventory_items item_id; Type: FK CONSTRAINT; Schema: public; Owner: hoangduy06104
--

ALTER TABLE ONLY public.rls_inventory_items
    ADD CONSTRAINT item_id FOREIGN KEY (item_id) REFERENCES public.tbl_items(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2422 (class 2606 OID 114739)
-- Name: rls_rental_items user; Type: FK CONSTRAINT; Schema: public; Owner: hoangduy06104
--

ALTER TABLE ONLY public.rls_rental_items
    ADD CONSTRAINT "user" FOREIGN KEY (user_id) REFERENCES public.tbl_users(id) ON UPDATE CASCADE ON DELETE CASCADE NOT VALID;


--
-- TOC entry 2580 (class 0 OID 0)
-- Dependencies: 5
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: hoangduy06104
--

REVOKE USAGE ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2024-01-01 18:01:51

--
-- PostgreSQL database dump complete
--

