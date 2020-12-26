USE [desk]
GO
/****** Object:  Schema [dia]    Script Date: 26.12.2020 17:32:38 ******/
CREATE SCHEMA [dia]
GO
/****** Object:  Table [dia].[Readings]    Script Date: 26.12.2020 17:32:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dia].[Readings](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[exam_id] [bigint] NOT NULL,
	[meter_id] [bigint] NOT NULL,
	[value] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dia].[Exams]    Script Date: 26.12.2020 17:32:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dia].[Exams](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[section_id] [int] NOT NULL,
	[filledFrom] [datetime] NOT NULL,
	[filledTo] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dia].[Sections]    Script Date: 26.12.2020 17:32:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dia].[Sections](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[sorter] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dia].[Meters]    Script Date: 26.12.2020 17:32:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dia].[Meters](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[device_id] [int] NOT NULL,
	[element_id] [int] NOT NULL,
	[line_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dia].[Users]    Script Date: 26.12.2020 17:32:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dia].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[active] [smallint] NOT NULL,
 CONSTRAINT [PK__Users__3213E83F37721479] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dia].[GetExamInfo]    Script Date: 26.12.2020 17:32:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE function [dia].[GetExamInfo] (@exam_id bigint)
returns table
as
return 
(
	 SELECT 
		s.name section_name, 
		ex.section_id, 
		ex.filledFrom, 
		ex.filledTo, 
		u.name [user_name],
		count(distinct (m.device_id*10000)+ m.element_id)+1 as cols, 
		count(distinct m.line_id)+2 rows
	 FROM dia.exams ex 	  
		inner join dia.Sections s on (s.id=ex.section_id)
		inner join dia.users u on (u.id=ex.[user_id])
		inner join dia.readings r on (ex.id=r.exam_id)
		inner join dia.Meters m on (r.meter_id=m.id)		
	  WHERE 
		ex.id=@exam_id 
	  GROUP BY s.name, ex.section_id, ex.filledFrom, ex.filledTo, u.name  
)



GO
/****** Object:  Table [dia].[SectionDeviceLinks]    Script Date: 26.12.2020 17:32:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dia].[SectionDeviceLinks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[section_id] [int] NOT NULL,
	[device_id] [int] NOT NULL,
	[sorter] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dia].[Lines]    Script Date: 26.12.2020 17:32:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dia].[Lines](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[section_id] [int] NOT NULL,
	[sorter] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dia].[Devices]    Script Date: 26.12.2020 17:32:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dia].[Devices](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dia].[Elements]    Script Date: 26.12.2020 17:32:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dia].[Elements](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[sorter] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dia].[GetReadings]    Script Date: 26.12.2020 17:32:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE function [dia].[GetReadings] (@exam_id bigint)
returns table
as
return 
(
	 SELECT distinct
		d.name device_name, 
		el.name element_name, 
		l.name line_name, 
		m.id meter_id,   -- убрать
		r.id reading_id, -- убрать
		isnull(cast(r.value as varchar(50)),'') value, -- значение прибора
		DENSE_RANK  ( ) OVER (ORDER BY (sdl.sorter*10000)+el.sorter) col_number,
		DENSE_RANK  ( ) OVER (ORDER BY l.sorter) row_number
	 FROM dia.exams ex 	  
		inner join dia.Users u on (ex.[user_id]=u.id)
		left join dia.Readings r on (ex.id=r.exam_id)
		inner join dia.Meters m on (r.meter_id=m.id)	
		inner join dia.Lines l on (m.line_id=l.id)
		inner join dia.Elements el on (m.element_id=el.id)
		inner join dia.Devices d on (m.device_id=d.id)
		inner join dia.SectionDeviceLinks sdl on (d.id=sdl.device_id)
	  WHERE 
		ex.id= @exam_id 		  
)


GO
/****** Object:  Table [dia].[DeviceElementLinks]    Script Date: 26.12.2020 17:32:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dia].[DeviceElementLinks](
	[device_id] [int] NOT NULL,
	[element_id] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dia].[GetMeters]    Script Date: 26.12.2020 17:32:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE function [dia].[GetMeters] (@section_id int)
returns table
as
return 
(
	SELECT 
		d.name device_name, 
		el.name element_name, 
		l.name line_name, 
		sdl.sorter device_sorter, 
		el.sorter element_sorter, 
		l.sorter line_sorter, 
		m.id meter_id,
		DENSE_RANK  ( ) OVER (ORDER BY (sdl.sorter*10000)+el.sorter) col_number,
		DENSE_RANK  ( ) OVER (ORDER BY l.sorter) row_number
	FROM dia.sectiondevicelinks sdl
		inner join dia.devices d on d.id=sdl.device_id
		inner join dia.deviceelementlinks del on d.id=del.device_id
		inner join dia.elements el on el.id=del.element_id
		inner join dia.lines l on l.section_id=sdl.section_id
		inner join dia.Meters m on (m.line_id=l.id and m.device_id=d.id and m.element_id=del.element_id)
	where sdl.section_id=@section_id  
)


GO
/****** Object:  UserDefinedFunction [dia].[GetExams]    Script Date: 26.12.2020 17:32:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE function [dia].[GetExams] (@report_date datetime)
returns table
as
return 
(
	 SELECT  
		ex.id exam_id, 
		s.name section_name, 
		ex.section_id, 
		ex.filledFrom, 
		ex.filledTo, 
		u.name [user_name],
		count(distinct (m.device_id*10000)+ m.element_id)+1 as cols, 
		count(distinct m.line_id)+2 rows
	 FROM dia.exams ex 	  
		inner join dia.Sections s on (s.id=ex.section_id)
		inner join dia.users u on (u.id=ex.[user_id])
		inner join dia.readings r on (ex.id=r.exam_id)
		inner join dia.Meters m on (r.meter_id=m.id)
	where format(ex.filledFrom,'d','zh-cn')=format(@report_date,'d', 'ru-ru' ) 
	GROUP BY ex.id, s.name, ex.section_id, ex.filledFrom, ex.filledTo, u.name  
)

GO
