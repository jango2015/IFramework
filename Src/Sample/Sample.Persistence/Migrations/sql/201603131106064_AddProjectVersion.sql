﻿ALTER TABLE [dbo].[Products] ADD [Version] rowversion NOT NULL
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201603131106064_AddProjectVersion', N'Sample.Persistence.Migrations.Configuration',  0x1F8B0800000000000400ED5CCD72E33612BE6FD5BE038BA764CB116DCF253B2527E591EDC49578ECB2EC5C533009C9A8214106001DEBD9F6B08FB4AFB0CD7F8000FF444A7626AAB95820F0A1D1E86E341ADDF3BFFFFC77FEE36BE05B2F987112D233FB64766C5B98BAA147E8FACC8EC5EABBEFED1F7FF8E73FE6975EF06AFD56F4FB90F48391949FD9CF42441F1D87BBCF38407C161097853C5C89991B060EF242E7F4F8F8DFCEC9898301C2062CCB9ADFC7549000A73FE0E722A42E8E448CFC9BD0C33ECFDBE1CB3245B53EA300F308B9F8CC5EA220F2F1EC2E21850BA015DBD6B94F1050B2C4FECAB610A5A14002E8FCF8C8F152B090AE97113420FF611361E8B7423EC739FD1FABEE7D97727C9A2CC5A90616506ECC45180C043CF990F3C6A90FDF8AC376C93BE0DE2570596C9255A71C3CB3CF5D3704D6DB567DAE8F0B9F25FD4AFE5EC0574267E97ECCF2614796F2F1A89407109BE4DF91B5887D11337C46712C18F28FACBBF8C927EE2F78F3107EC1F48CC6BE2F530834C237A5019AEE5818612636F77895D37D7D615B8E3ACEA90F2C874963B225FD1413CFB63EC3DCE8C9C7E5FE3BADC32F61897E810052040A615B37E8F5574CD7E2F9CC863F6DEB8ABC62AF68C9611F2901FD814182C59DB3FC1C46D166E7B3801EB0E4AF9D4F748738FF3364DECE27BAC7EB44FBD903A956758104CE7E0FDCEA5C8C0B9C6B2A3E9CDA56629462C6C0C26C6E52BA52A23BC0E74EA573AD9AB80803503FAF4D13AFAF187405767E99DD60CED11A8C5998F073960F3EB29ABB6413969FB7D555EB13E238DF4BB0AEC52C762B3F97B092989732208D2BBE0CD545C642B6739902E2DC2F0F0CB953EB496FA128B8BB9550E43FF72114E74F1CBEBA225FECDE0DBA615F4E4EBFEFB52F03256F118205F0D33D9864FAF6D9F2EDFA147ABB3F13160C97F672A4FDDCCBD192C04F3FC967F442D6E9F6D6D9F34C7C0F8C7FA992F7B918F06712A946ED77ADEF150B83FBD0AF76B4DEE5F76518B3D4CE84EDFD1E105B63D19FEC3B0483C58E882E88E922BA58DC5636F0F205B73BA82DE62D19FA5E0FC5F3F59A61D8337C1F86A2D5964CA32FCA7CBB519E5E6ED434EED21538E33F83CBE3636F8480D451DE405694451C9C9EE40A347A5BF7BCA5DBB83B5E7D1F8CDD96F11377198992856787EA601FA9EDEAB5231F49A77ADF14E402B08533D35B4A61462F7607474EF2616F1839D1246F88106D1539D98B3BB8C8E258FD8F9A9D7AC0B5A3EF13A1886D9A430812371AF4419B5F12B8C49CE19F30C50CC8F5EE901098D134A210C5027B5B0AF8234D258E3F8F32C575943738611FE95276819D8E55279D4705631488BFC67A47C519148843B4E12DA20DD398D043B4A17992BD5C981EC288B87B73BBE3408F3416F1D86B7EE5A375F5343622FE9841EEC32E3C520F337F0390B217A072FC06074F98553177F0C1D223F237E4C7D072AC6D913220B92F4ABD4F7456674C951BCF390F5D92724DE5B516715127BEA49ED53366A4471EAC1BE018898047C05958D66C76A2ADAC7B8232BED339C1BF347430B638093D11E483D793186C42856E9909754984FC7E2BAD0DEF69DA938D2927AA7FB9C01106B1A1A21F2BFA505033DA3A31E59CB513A88B6573479225FD548731024694C29ADD30D26B47F209BF9A5CB8478E7355E6C6E334835E62A13E188389A8CC4A2E1AE55BB226672A44CE521384D1693140C8177C138E1A42E800CBAF64269CF292D701A1B84026A0169F4C35111A9BF4B8ACD4BB23845B97BCFE26A55CA0BC5D9A24F7B7203DF014E9869E8633AC94F02A1FC4C912428AC411A72173647E83A2080E062993246FB196591AC9E2BBE5F0FC8A20C3705C6E48B328A92D6782F30E565EFB9A180C0F5F11C605B840E80925E7EDC20BB46E067D6E90CA62C29ACAEADB5748693120F95B361E72564DA9DFBA0DCC87C309BE0E12539A5E48A52D6F1C9926F4201F3183AFBD08FD38A0CD46BD79749EA92103E44DFD31F23C0C19236FEA8F516559C830556B7FA42A8D4246AA5AFB23A97912329AFAA53F6219EA90C1CA461D67EED4C4453B1F35B9ACD989BA9CF7D282D2EC8CD4822234305C0BF2911A05CDAC1DA706353F4406EA70519A31952BA28CA87C1840A3740D540894DAFBA3E9CA3654D1B26B9E8C90B5F447286E503246D136C084656F3D8A09CB9A065152BEE4D4A829DBA757CFE11A6578FA4A61BAF429F7FE0EDAF4156B93F6482E83691FB7C4D5C9347CFEEA0E44F50E355287B5E7F8E1EADC3EBC51A96B3E91E961B579B4FE52A918CA8ED7D736E4B733E0569A3EEF9134E09407AAF46C0919EF8224CB0C0845C24CE08ECF846B9EFC7DBBFAC6205832D5DFFEEDA54A79D756EE26F287F770A8FF5D8D81A67A6FA8765B1E0C653C6CA40814C1B3E1BBDF3872371B3FDED1C9930F54A7D0100B6DC598D479FB8B3821B5C0E94889D35216868B5E1DE270BFF8AAEF17F98BAB029135BD8743544D46D94698A5F107493E487281B04B63AF3DD6D4BB94B3978F36B5C79979FE50D25DFBABBD9C645D6C0B98F342BCF4D56403CA14CC920EB3E51FFEC227A9235674B84194AC3017591E817D7A7C725A2B1F7E3FA5BC0EE79E6F786832D4F3AADBB5879CA898923F624C92576DB222988D2BAFA52F88B9CF887D13A0D76F65A8C125B4A390EA65B2A3C0EAA5B0A3C04CE5AE1E582E3141B22A49F4639A24E996E286F79A265F6C8B9EDE375116FC64131892DCB71601A5B6649464EAF523A3E06A97E4512C1C9CDB3F89C40E11BED15674326BA5A4D3775B04038096E039957D62E19F2F454B67EAFBA4C5F97CFFE7EA64D6C2984ADC077DCBCCE171D2379DF04CA6117206F0580B29FD6F08DBE8D63BB1D6C34A780FCA73509EF1400D55D3D3614E46E9EEBCEA8E18E5D7A3595BF1FDA04E4380E48A935D98FEB610E441500F82FAB6825A2F99D033B11B5EB16AE1B69662882C22097BF11402F919995511C5803A87CE3207D34CB542897E95106D8510A649AA028A0EFCD20CE813549F4C33545F3B6668AC08293E98D07B72473F764D751E5A1FD38C866EBD2A4C5A1858EF609E56ED6352941E35167A787EEEC8FF81E7FC0273B2AE2092A4088A5DA56AA9EC734D5761A1EBB5AA8FA28B66370502CB86CE99202BE40AF8EC82294D8B25F3A48BCBE0097BD7F43616512CCE39C7C193AFBC0F2545586DF3A785242ACDF3DB3494C7A75802904912E37C4B3FC5C4AF0A0AAF0C5E61034462AAF2C047522F239200C87A53227D0E694FA09C7D6551DA030E2238E730BFA54BF482B7A1ED91E35FC1A17637C52B4B3348F746A86C9F5F10B46628E03946351E7E820C7BC1EB0FFF07F38CE218C7560000 , N'6.1.3-40302')
