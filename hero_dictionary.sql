-- MySQL dump 10.13  Distrib 8.0.23, for Win64 (x86_64)
--
-- Host: localhost    Database: hero
-- ------------------------------------------------------
-- Server version	8.0.23

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `dictionary`
--

DROP TABLE IF EXISTS `dictionary`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dictionary` (
  `id` int NOT NULL AUTO_INCREMENT,
  `dictionary_keys_id` int NOT NULL DEFAULT '0',
  `langs_id` int NOT NULL DEFAULT '0',
  `value` varchar(45) COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_dictionary_langs_idx` (`langs_id`),
  KEY `fk_dictionary_dictionary_keys1_idx` (`dictionary_keys_id`),
  CONSTRAINT `fk_dictionary_dictionary_keys1` FOREIGN KEY (`dictionary_keys_id`) REFERENCES `dictionary_keys` (`id`),
  CONSTRAINT `fk_dictionary_langs` FOREIGN KEY (`langs_id`) REFERENCES `langs` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dictionary`
--

LOCK TABLES `dictionary` WRITE;
/*!40000 ALTER TABLE `dictionary` DISABLE KEYS */;
INSERT INTO `dictionary` VALUES (1,1,1,'Музыка'),(2,1,2,'Music'),(3,2,1,'Звуки'),(4,2,2,'Sound'),(5,3,1,'Язык'),(6,3,2,'Language'),(7,4,1,'Качество графики'),(8,4,2,'Graphics quality'),(9,5,1,'Оценить нас'),(10,5,2,'Rate us'),(11,6,1,'Войти'),(12,6,2,'Sign In'),(13,7,1,'Зарегистрироваться'),(14,7,2,'Sign Up'),(15,8,1,'Имя'),(16,8,2,'First name'),(17,9,1,'Фамилия'),(18,9,2,'Last name'),(19,10,1,'Почта'),(20,10,2,'Email'),(21,11,1,'Пароль'),(22,11,2,'Password'),(23,12,1,'Захватить территорию'),(24,12,2,'Capture territory'),(25,13,1,'Купить 5 героев'),(26,13,2,'Buy 5 heroes'),(27,14,1,'Прокачать максимальный уровень у любого героя'),(28,14,2,'Upgrade the maximum level of any hero'),(29,15,1,'ЧЕЛОВЕК'),(30,15,2,'HUMAN'),(31,16,1,'Броня'),(32,16,2,'Armor'),(33,17,1,'Вы имеете'),(34,17,2,'You have');
/*!40000 ALTER TABLE `dictionary` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-05-16  9:52:10
