using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

//namespace HDIC.SysRole
//{
//    /// <summary>
//    /// 实体类SysRole
//    /// </summary> 
//    [Serializable]
//    [Table(TableName = "SysRole", IsTable = true)]

//    class SysRole
//    {
//        /// <summary>
//        /// 构造函数
//        /// </summary>
//        public SysRole()
//        {

//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        private string _roleno = "";
//        /// <summary>
//        /// 角色名称
//        /// </summary>
//        private string _rolename = "";
//        /// <summary>
//        /// 是否有效 1有效0无效
//        /// </summary>
//        private bool _iseffect = false;
//        /// <summary>
//        /// 是否系统角色 1是0否
//        /// </summary>
//        private bool _issys = false;
//        /// <summary>
//        /// 
//        /// </summary>
//        private string _remark = "";

//        /// <summary>
//        /// 
//        /// </summary>
//        [Column(FieldName = "ROLENO", Type = DbType.String, Length = 20, IsPrimaryKey = true)]
//        public string RoleNo
//        {
//            get { return _roleno; }
//            set { _roleno = value; }
//        }
//        /// <summary>
//        /// 角色名称
//        /// </summary>
//        [Column(FieldName = "ROLENAME", Type = DbType.String, Length = 20)]
//        public string RoleName
//        {
//            get { return _rolename; }
//            set { _rolename = value; }
//        }
//        /// <summary>
//        /// 是否有效 1有效0无效
//        /// </summary>
//        [Column(FieldName = "ISEFFECT", Type = DbType.Boolean, Length = 1)]
//        public bool IsEffect
//        {
//            get { return _iseffect; }
//            set { _iseffect = value; }
//        }
//        /// <summary>
//        /// 是否系统角色 1是0否
//        /// </summary>
//        [Column(FieldName = "ISSYS", Type = DbType.Boolean, Length = 1)]
//        public bool IsSys
//        {
//            get { return _issys; }
//            set { _issys = value; }
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        [Column(FieldName = "REMARK", Type = DbType.String, Length = 100)]
//        public string Remark
//        {
//            get { return _remark; }
//            set { _remark = value; }
//        }
//    }
//}
