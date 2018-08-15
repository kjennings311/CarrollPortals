-- phpMyAdmin SQL Dump
-- version 4.6.5.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jul 02, 2018 at 08:20 AM
-- Server version: 10.1.21-MariaDB
-- PHP Version: 5.6.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `cvs`
--

-- --------------------------------------------------------

--
-- Table structure for table `phpro_users`
--

CREATE TABLE `phpro_users` (
  `phpro_user_id` int(10) NOT NULL,
  `phpro_password` varchar(20) NOT NULL,
  `phpro_username` varchar(30) NOT NULL,
  `DESIGNATION` varchar(30) NOT NULL,
  `BRCODE` int(10) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dumping data for table `phpro_users`
--

INSERT INTO `phpro_users` (`phpro_user_id`, `phpro_password`, `phpro_username`, `DESIGNATION`, `BRCODE`) VALUES
(1, '1', 'Aravind', 'CHAIRMAN', 1),
(2, '2', 'Surender', 'GM', 1),
(3, '3', 'Yesupadam', 'GM', 1),
(4, '4', 'Srinivas', 'GM', 1),
(5, '5', 'Ravinder', 'RM', 902),
(6, '6', 'Ramchander', 'RM', 777),
(7, '7', 'Bhasker', 'BM', 143),
(8, '8', 'Jagadish', 'BM', 144),
(9, '9', 'Sai', 'BM', 186),
(10, '10', 'Sayanna', 'AO(ADV)', 902),
(11, '11', 'Santosh', 'FO', 144),
(12, '12', 'Sampath', 'IT', 1);

-- --------------------------------------------------------

--
-- Table structure for table `region`
--

CREATE TABLE `region` (
  `Bcode` int(5) NOT NULL,
  `BRANCHNAME` varchar(30) NOT NULL,
  `CO` varchar(30) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dumping data for table `region`
--

INSERT INTO `region` (`Bcode`, `BRANCHNAME`, `CO`) VALUES
(1, 'Head Office', 'Head Office'),
(902, 'RO Hyderabad2', 'Hyderabad2'),
(777, 'RO Nizamabad', 'Nizamabad'),
(143, 'Kamareddy', 'Nizamabad'),
(144, 'Armoor', 'Nizamabad'),
(186, 'Turkapally', 'Hyderabad2');

-- --------------------------------------------------------

--
-- Stand-in structure for view `view_allvisits`
-- (See below for the actual view)
--
CREATE TABLE `view_allvisits` (
`id` int(11)
,`controlleruserid` int(11)
,`controllername` varchar(30)
,`DESIGNATION` varchar(30)
,`region` varchar(30)
,`VisitDate` date
,`brachname` varchar(30)
,`BranchManager` varchar(30)
,`dateofcompliance` varchar(10)
,`dateofclosure` varchar(10)
,`bmuserid` int(11)
,`status` varchar(40)
,`statusid` int(11)
,`branchid` int(11)
,`dateofvisit` varchar(10)
);

-- --------------------------------------------------------

--
-- Table structure for table `visits`
--

CREATE TABLE `visits` (
  `id` int(11) NOT NULL,
  `controlleruserid` int(11) NOT NULL,
  `designation` varchar(30) COLLATE utf8_unicode_ci NOT NULL,
  `region` varchar(30) COLLATE utf8_unicode_ci NOT NULL,
  `visitbranch` int(11) NOT NULL,
  `bmuserid` int(11) NOT NULL,
  `dateofvisit` date NOT NULL,
  `dateofcompliance` date NOT NULL,
  `dateofclosure` date NOT NULL,
  `visitstatus` int(11) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `visits`
--

INSERT INTO `visits` (`id`, `controlleruserid`, `designation`, `region`, `visitbranch`, `bmuserid`, `dateofvisit`, `dateofcompliance`, `dateofclosure`, `visitstatus`) VALUES
(1, 5, 'RM', 'Hyderabad2', 186, 9, '2018-01-07', '0000-00-00', '0000-00-00', 2),
(2, 5, 'RM', 'Hyderabad2', 186, 9, '2018-01-07', '2018-07-01', '2018-07-01', 4),
(3, 5, 'RM', 'Hyderabad2', 186, 9, '2018-01-07', '0000-00-00', '0000-00-00', 2),
(4, 5, 'RM', 'Hyderabad2', 186, 9, '2018-01-07', '2018-07-01', '0000-00-00', 3),
(5, 5, 'RM', 'Hyderabad2', 186, 9, '2018-01-07', '0000-00-00', '0000-00-00', 2),
(6, 5, 'RM', 'Hyderabad2', 186, 9, '2018-01-07', '0000-00-00', '0000-00-00', 2),
(7, 5, 'RM', 'Hyderabad2', 186, 9, '2018-01-07', '2018-07-01', '2018-07-01', 4),
(8, 5, 'RM', 'Hyderabad2', 186, 9, '2018-01-07', '0000-00-00', '0000-00-00', 2),
(9, 5, 'RM', 'Hyderabad2', 186, 9, '2018-01-07', '0000-00-00', '0000-00-00', 2),
(10, 5, 'RM', 'Hyderabad2', 186, 9, '2018-01-07', '2018-07-01', '2018-07-01', 4),
(11, 5, 'RM', 'Hyderabad2', 186, 9, '2018-07-04', '2018-07-01', '2018-07-01', 4),
(12, 5, 'RM', 'Hyderabad2', 186, 9, '2018-07-04', '2018-07-01', '2018-07-01', 4),
(13, 5, 'RM', 'Hyderabad2', 186, 9, '2018-07-04', '2018-07-02', '0000-00-00', 3),
(14, 5, 'RM', 'Hyderabad2', 186, 9, '2018-07-04', '2018-07-02', '2018-07-02', 4);

-- --------------------------------------------------------

--
-- Table structure for table `visit_observations_compliances`
--

CREATE TABLE `visit_observations_compliances` (
  `id` int(11) NOT NULL,
  `visitid` int(11) NOT NULL,
  `paramid` text COLLATE utf8_unicode_ci NOT NULL,
  `observation` text COLLATE utf8_unicode_ci NOT NULL,
  `observationdate` date NOT NULL,
  `Compliancetext` text COLLATE utf8_unicode_ci NOT NULL,
  `compliancedate` date NOT NULL,
  `closureremarks` text COLLATE utf8_unicode_ci NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `visit_observations_compliances`
--

INSERT INTO `visit_observations_compliances` (`id`, `visitid`, `paramid`, `observation`, `observationdate`, `Compliancetext`, `compliancedate`, `closureremarks`) VALUES
(1, 1, 'Parameter 1', ' asdf', '2018-07-01', '', '0000-00-00', ''),
(2, 1, 'parameter 2', ' asdf', '2018-07-01', '', '0000-00-00', ''),
(3, 1, 'Parameter 3', ' adsf', '2018-07-01', '', '0000-00-00', ''),
(4, 1, 'parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', ' asdf', '2018-07-01', '', '0000-00-00', ''),
(5, 1, 'parameter 5', ' adf', '2018-07-01', '', '0000-00-00', ''),
(6, 1, 'parameter 6', ' asdf', '2018-07-01', '', '0000-00-00', ''),
(7, 1, 'parameter 7', ' asdf', '2018-07-01', '', '0000-00-00', ''),
(8, 1, 'parameter 8', ' asdf', '2018-07-01', '', '0000-00-00', ''),
(9, 2, 'Parameter 1', '   adsf  ', '2018-07-01', '  no disabled me   adsf  adfasdfasdf', '2018-07-01', ''),
(10, 2, 'parameter 2', '   asdf  ', '2018-07-01', '  no disabled me   adsfa  dsasfadsf', '2018-07-01', ''),
(11, 2, 'Parameter 3', '    ads ', '2018-07-01', '  sdf', '2018-07-01', ''),
(12, 2, 'parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '    adsf ', '2018-07-01', '  asdf', '2018-07-01', ''),
(13, 2, 'parameter 5', '    adf ', '2018-07-01', '  adsf', '2018-07-01', ''),
(14, 2, 'parameter 6', '    asd ', '2018-07-01', '  adf', '2018-07-01', ''),
(15, 2, 'parameter 7', '    asdf  asdf', '2018-07-01', '  adf', '2018-07-01', ''),
(16, 2, 'parameter 8', '    asdf a asfdsdf', '2018-07-01', '  asdf', '2018-07-01', ''),
(18, 3, 'parameter 2', '  asdf ', '2018-07-01', '', '0000-00-00', ''),
(17, 3, 'Parameter 1', '  asdf ', '2018-07-01', '', '0000-00-00', ''),
(19, 3, 'Parameter 3', '  asdf ', '2018-07-01', '', '0000-00-00', ''),
(20, 3, 'parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '   adsf', '2018-07-01', '', '0000-00-00', ''),
(21, 3, 'parameter 5', '   adsf', '2018-07-01', '', '0000-00-00', ''),
(22, 3, 'parameter 6', '   adsf', '2018-07-01', '', '0000-00-00', ''),
(23, 3, 'parameter 7', '   asdf', '2018-07-01', '', '0000-00-00', ''),
(24, 3, 'parameter 8', '   adf', '2018-07-01', '', '0000-00-00', ''),
(25, 4, 'Parameter 1', ' asdf', '2018-07-01', '    asdf', '2018-07-01', ''),
(26, 4, 'parameter 2', ' asdf', '2018-07-01', '    asdf', '2018-07-01', ''),
(27, 4, 'Parameter 3', ' asdf', '2018-07-01', '    asdf', '2018-07-01', ''),
(28, 4, 'parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', ' asdf', '2018-07-01', '    asdf', '2018-07-01', ''),
(29, 4, 'parameter 5', ' asdf', '2018-07-01', '    asfd', '2018-07-01', ''),
(30, 4, 'parameter 6', ' asdf', '2018-07-01', '    asdf', '2018-07-01', ''),
(31, 4, 'parameter 7', ' asdf', '2018-07-01', '    asdf', '2018-07-01', ''),
(32, 4, 'parameter 8', ' asd', '2018-07-01', '    asdf', '2018-07-01', ''),
(33, 5, 'Parameter 1', ' adf', '2018-07-01', '', '0000-00-00', ''),
(34, 5, 'parameter 2', ' asdfa', '2018-07-01', '', '0000-00-00', ''),
(35, 5, 'Parameter 3', ' sdfa', '2018-07-01', '', '0000-00-00', ''),
(36, 5, 'parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', ' sdf', '2018-07-01', '', '0000-00-00', ''),
(37, 5, 'parameter 5', ' asdf', '2018-07-01', '', '0000-00-00', ''),
(38, 5, 'parameter 6', ' asdf', '2018-07-01', '', '0000-00-00', ''),
(39, 5, 'parameter 7', ' asdf', '2018-07-01', '', '0000-00-00', ''),
(40, 5, 'parameter 8', ' adfasdf', '2018-07-01', '', '0000-00-00', ''),
(41, 6, 'Parameter 1', '  adsf ', '2018-07-01', '', '0000-00-00', ''),
(42, 6, 'parameter 2', '  adsfa ', '2018-07-01', '', '0000-00-00', ''),
(43, 6, 'Parameter 3', '  asdf ', '2018-07-01', '', '0000-00-00', ''),
(44, 6, 'parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '  asdf ', '2018-07-01', '', '0000-00-00', ''),
(45, 6, 'parameter 5', '  asdf ', '2018-07-01', '', '0000-00-00', ''),
(46, 6, 'parameter 6', '  asdf ', '2018-07-01', '', '0000-00-00', ''),
(47, 6, 'parameter 7', '  asdf ', '2018-07-01', '', '0000-00-00', ''),
(48, 6, 'parameter 8', '  asdf ', '2018-07-01', '', '0000-00-00', ''),
(49, 7, 'Parameter 1', '    asdf   ', '2018-07-01', '   asdf  asfasdf', '2018-07-01', '   sfd '),
(50, 7, 'parameter 2', '    asdf   ', '2018-07-01', '   asdfas  asdfsadf', '2018-07-01', '   sdf '),
(51, 7, 'Parameter 3', '     asdf  ', '2018-07-01', '   asdf  asdfasdf', '2018-07-01', '   sdf '),
(52, 7, 'parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '     asdf  ', '2018-07-01', '  asdf', '2018-07-01', '    '),
(53, 7, 'parameter 5', '     asdf  ', '2018-07-01', '  asf', '2018-07-01', '    '),
(54, 7, 'parameter 6', '     asdf  ', '2018-07-01', '  asdf', '2018-07-01', '    '),
(55, 7, 'parameter 7', '     asdf  ', '2018-07-01', '  asdf', '2018-07-01', '    '),
(56, 7, 'parameter 8', '     asdf  ', '2018-07-01', '  afd', '2018-07-01', '    '),
(57, 8, 'Parameter 1', '  dfsa ', '2018-07-01', '', '0000-00-00', ''),
(58, 8, 'parameter 2', '  asdf ', '2018-07-01', '', '0000-00-00', ''),
(59, 8, 'Parameter 3', '  asdf ', '2018-07-01', '', '0000-00-00', ''),
(60, 8, 'parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '  adsf ', '2018-07-01', '', '0000-00-00', ''),
(61, 8, 'parameter 5', '   asdf', '2018-07-01', '', '0000-00-00', ''),
(62, 8, 'parameter 6', '   asdf', '2018-07-01', '', '0000-00-00', ''),
(63, 8, 'parameter 7', '   asdf', '2018-07-01', '', '0000-00-00', ''),
(64, 8, 'parameter 8', '   asdf', '2018-07-01', '', '0000-00-00', ''),
(65, 9, 'Parameter 1', '  asdf ', '2018-07-02', '', '0000-00-00', ''),
(66, 9, 'parameter 2', '  asd ', '2018-07-02', '', '0000-00-00', ''),
(67, 9, 'Parameter 3', '  asdf ', '2018-07-02', '', '0000-00-00', ''),
(68, 9, 'parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '  asdf ', '2018-07-02', '', '0000-00-00', ''),
(69, 9, 'parameter 5', '  asdf ', '2018-07-02', '', '0000-00-00', ''),
(70, 9, 'parameter 6', '  asdfa ', '2018-07-02', '', '0000-00-00', ''),
(71, 9, 'parameter 7', '  sdf ', '2018-07-02', '', '0000-00-00', ''),
(72, 9, 'parameter 8', '  adsf ', '2018-07-02', '', '0000-00-00', ''),
(73, 10, 'Parameter 1', '  afd ', '2018-07-01', '  as', '2018-07-01', ''),
(74, 10, 'parameter 2', '  asdf ', '2018-07-01', '  asdf', '2018-07-01', ''),
(75, 10, 'Parameter 3', '  asdf ', '2018-07-01', '  asdf', '2018-07-01', ''),
(76, 10, 'parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '   asdf', '2018-07-01', '  asdf', '2018-07-01', ''),
(77, 10, 'parameter 5', '   asdf', '2018-07-01', '  asdf', '2018-07-01', ''),
(78, 10, 'parameter 6', '   asdf', '2018-07-01', '  asdf', '2018-07-01', ''),
(79, 10, 'parameter 7', '   asdf', '2018-07-01', '  asf', '2018-07-01', ''),
(80, 10, 'parameter 8', '   asdf', '2018-07-01', '  asdf', '2018-07-01', ''),
(81, 11, 'Parameter 1', '  adsf ', '2018-07-01', '   asdf afd', '2018-07-01', ''),
(82, 11, 'parameter 2', '  adf ', '2018-07-01', '   asdf asfd', '2018-07-01', ''),
(83, 11, 'Parameter 3', '  asdf ', '2018-07-01', '  adf', '2018-07-01', ''),
(84, 11, 'parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '   asdf', '2018-07-01', '  asdf', '2018-07-01', ''),
(85, 11, 'parameter 5', '   adsf', '2018-07-01', '  asdf', '2018-07-01', ''),
(86, 11, 'parameter 6', '   adf', '2018-07-01', '  asdf', '2018-07-01', ''),
(87, 11, 'parameter 7', '   asdf', '2018-07-01', '  asf', '2018-07-01', ''),
(88, 11, 'parameter 8', '   asfd', '2018-07-01', '  asfd', '2018-07-01', ''),
(89, 12, 'Parameter 1', ' sdfg', '2018-07-01', '  sdgsdg', '2018-07-01', '  sdg'),
(90, 12, 'parameter 2', ' sdfg', '2018-07-01', '  sdfg', '2018-07-01', '  '),
(91, 12, 'Parameter 3', ' sdfg', '2018-07-01', '  sdfg', '2018-07-01', '  '),
(92, 12, 'parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', ' sdfg', '2018-07-01', '  sdfg', '2018-07-01', '  '),
(93, 12, 'parameter 5', ' sg', '2018-07-01', '  sdfg', '2018-07-01', '  '),
(94, 12, 'parameter 6', ' sdfg', '2018-07-01', '  sdfg', '2018-07-01', '  '),
(95, 12, 'parameter 7', ' sdfg', '2018-07-01', '  sdg', '2018-07-01', '  '),
(96, 12, 'parameter 8', ' sdg', '2018-07-01', '  sdg', '2018-07-01', '  '),
(97, 13, 'Parameter 1', '  meter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 ', '2018-07-02', '  parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', ''),
(98, 13, 'parameter 2', '  meter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 ', '2018-07-02', '  parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', ''),
(99, 13, 'Parameter 3', '  meter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 ', '2018-07-02', '  parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', ''),
(100, 13, 'parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '  meter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 ', '2018-07-02', '  parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', ''),
(101, 13, 'parameter 5', '  meter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 ', '2018-07-02', '  parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', ''),
(102, 13, 'parameter 6', '  meter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 ', '2018-07-02', '  parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', ''),
(103, 13, 'parameter 7', '  meter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 ', '2018-07-02', '  parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', ''),
(104, 13, 'parameter 8', '  meter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4meter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4meter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4meter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 ', '2018-07-02', '  parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', ''),
(105, 14, 'Parameter 1', 'parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', '  parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', '  '),
(106, 14, 'parameter 2', ' parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', '  parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', '  '),
(107, 14, 'Parameter 3', ' parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', '  parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', '  ');
INSERT INTO `visit_observations_compliances` (`id`, `visitid`, `paramid`, `observation`, `observationdate`, `Compliancetext`, `compliancedate`, `closureremarks`) VALUES
(108, 14, 'parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', ' parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', '  parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', '  '),
(109, 14, 'parameter 5', ' parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', '  parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', '  '),
(110, 14, 'parameter 6', ' parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', '  parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', '  '),
(111, 14, 'parameter 7', ' parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', '  parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', '  '),
(112, 14, 'parameter 8', ' parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', '  parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4', '2018-07-02', '  ');

-- --------------------------------------------------------

--
-- Table structure for table `visit_parameters`
--

CREATE TABLE `visit_parameters` (
  `id` int(11) NOT NULL,
  `parameter` text COLLATE utf8_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `visit_parameters`
--

INSERT INTO `visit_parameters` (`id`, `parameter`) VALUES
(1, 'Parameter 1'),
(2, 'parameter 2'),
(3, 'Parameter 3'),
(4, 'parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4 parameter 4 parameter 4 parameter 4 parameter 4v parameter 4'),
(5, 'parameter 5'),
(6, 'parameter 6'),
(7, 'parameter 7'),
(8, 'parameter 8');

-- --------------------------------------------------------

--
-- Table structure for table `visit_status`
--

CREATE TABLE `visit_status` (
  `id` int(11) NOT NULL,
  `status` varchar(40) COLLATE utf8_unicode_ci NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `visit_status`
--

INSERT INTO `visit_status` (`id`, `status`) VALUES
(1, 'Observations Not Submitted'),
(2, 'Pending for Compliance'),
(3, 'Pending for Closure'),
(4, 'Closure'),
(5, 'Resent for the Complaince'),
(6, 'Delete');

-- --------------------------------------------------------

--
-- Structure for view `view_allvisits`
--
DROP TABLE IF EXISTS `view_allvisits`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_allvisits`  AS  (select `v`.`id` AS `id`,`v`.`controlleruserid` AS `controlleruserid`,(select `u`.`phpro_username` from `phpro_users` `u` where (`u`.`phpro_user_id` = `v`.`controlleruserid`)) AS `controllername`,`v`.`designation` AS `DESIGNATION`,`v`.`region` AS `region`,`v`.`dateofvisit` AS `VisitDate`,(select `region`.`BRANCHNAME` from `region` where (`region`.`Bcode` = `v`.`visitbranch`)) AS `brachname`,(select `bu`.`phpro_username` from `phpro_users` `bu` where (`bu`.`phpro_user_id` = `v`.`bmuserid`)) AS `BranchManager`,date_format(`v`.`dateofcompliance`,'%d-%m-%Y') AS `dateofcompliance`,date_format(`v`.`dateofclosure`,'%d-%m-%Y') AS `dateofclosure`,`v`.`bmuserid` AS `bmuserid`,(select `vs`.`status` from `visit_status` `vs` where (`vs`.`id` = `v`.`visitstatus`)) AS `status`,`v`.`visitstatus` AS `statusid`,`v`.`visitbranch` AS `branchid`,date_format(`v`.`dateofvisit`,'%d-%m-%Y') AS `dateofvisit` from `visits` `v`) ;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `phpro_users`
--
ALTER TABLE `phpro_users`
  ADD PRIMARY KEY (`phpro_user_id`);

--
-- Indexes for table `visits`
--
ALTER TABLE `visits`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `visit_observations_compliances`
--
ALTER TABLE `visit_observations_compliances`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `visit_parameters`
--
ALTER TABLE `visit_parameters`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `visit_status`
--
ALTER TABLE `visit_status`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `visits`
--
ALTER TABLE `visits`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;
--
-- AUTO_INCREMENT for table `visit_observations_compliances`
--
ALTER TABLE `visit_observations_compliances`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=113;
--
-- AUTO_INCREMENT for table `visit_parameters`
--
ALTER TABLE `visit_parameters`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;
--
-- AUTO_INCREMENT for table `visit_status`
--
ALTER TABLE `visit_status`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
