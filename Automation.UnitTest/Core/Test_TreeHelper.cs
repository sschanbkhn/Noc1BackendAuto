using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network.Core.Abstracts;
using Network.Core.Core;
using Network.Core.Helpers;
using Network.UnitTest;
using Xunit;

namespace Homes.UnitTest.Core
{
    public class Test_TreeHelpers:IClassFixture<DatabaseFixture>
    {
        DatabaseFixture _fixture;
        public Test_TreeHelpers(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        public void ListToTrees()
        {
            var organs = _fixture.Db.Sys_Organizations.Select(o => new TreeTest() { Id = o.Id.ToString(), Code = o.Code, Name = o.Name, ParentId = o.ParentId.ToString() }).ToList();
            var trees = TreeHelpers<TreeTest>.ListToTrees(organs);          
            if(trees != null && trees.Count > 0 && trees[0].Children.Count > 0)
            {
                Assert.True(true);
            }   
            else
            {
                Assert.True(false);
            }    
        }
        [Theory]
        [InlineData("11111111-1111-1111-1111-111111111012")]
        [InlineData("11111111-1111-1111-1111-111111111022")]
        public void GetNodeFromTree(string nodeId)
        {
            var organs = _fixture.Db.Sys_Organizations.Select(o => new TreeTest() { Id = o.Id.ToString(), Code = o.Code, Name = o.Name, ParentId = o.ParentId.ToString() }).ToList();
            var trees = TreeHelpers<TreeTest>.ListToTrees(organs);
            bool nodeExisted = false;
            if (trees != null && trees.Count > 0 && trees[0].Children.Count > 0)
            {
                TreeTest node = null;
                for(int i = 0;i < trees.Count;i++)
                {
                    node = TreeHelpers<TreeTest>.GetNodeFromTree(trees[i], nodeId);
                    if(node != null)
                    {
                        nodeExisted = true;
                    }    
                }    
                if(nodeExisted)
                {
                    Assert.True(true);
                }   
                else
                {
                    Assert.True(false);
                }    
            }
            else
            {
                Assert.True(false);
            }            
        }

        [Theory]
        [InlineData("11111111-1111-1111-1111-111111111001")]
        [InlineData("11111111-1111-1111-1111-111111111002")]
        public void InsertNodeIntoTree(string nodeId)
        {            
            var organs = _fixture.Db.Sys_Organizations.Select(o => new TreeTest() { Id = o.Id.ToString(), Code = o.Code, Name = o.Name, ParentId = o.ParentId.ToString() }).ToList();
            var trees = TreeHelpers<TreeTest>.ListToTrees(organs);
            if (trees != null && trees.Count > 0 && trees[0].Children.Count > 0)
            {
                for (int i = 0; i < trees.Count; i++)
                {
                    TreeHelpers<TreeTest>.InsertNodeIntoTree(trees[i], nodeId, new TreeTest() { Id = Guid.NewGuid().ToString(), Code = "InsertNodeIntoTree", Name = "InsertNodeIntoTree" });
                }
            }
            else
            {
                Assert.True(false);
            }
        }

        [Theory]
        [InlineData("11111111-1111-1111-1111-111111111001")]
        [InlineData("11111111-1111-1111-1111-111111111002")]
        public void UpdateNodeIntoTree(string nodeId)
        {
            var organs = _fixture.Db.Sys_Organizations.Select(o => new TreeTest() { Id = o.Id.ToString(), Code = o.Code, Name = o.Name, ParentId = o.ParentId.ToString() }).ToList();
            var trees = TreeHelpers<TreeTest>.ListToTrees(organs);
            if (trees != null && trees.Count > 0 && trees[0].Children.Count > 0)
            {
                for (int i = 0; i < trees.Count; i++)
                {
                    TreeHelpers<TreeTest>.UpdateNodeIntoTree(trees[i], nodeId, new TreeTest() { Code = "UpdateNodeIntoTree", Name = "UpdateNodeIntoTree" });
                }
            }
            else
            {
                Assert.True(false);
            }
        }

        [Theory]
        [InlineData("11111111-1111-1111-1111-111111111001")]
        [InlineData("11111111-1111-1111-1111-111111111002")]
        public void DeleteNodeInTree(string nodeId)
        {
            var organs = _fixture.Db.Sys_Organizations.Select(o => new TreeTest() { Id = o.Id.ToString(), Code = o.Code, Name = o.Name, ParentId = o.ParentId.ToString() }).ToList();
            var trees = TreeHelpers<TreeTest>.ListToTrees(organs);
            if (trees != null && trees.Count > 0 && trees[0].Children.Count > 0)
            {
                for (int i = 0; i < trees.Count; i++)
                {
                    TreeHelpers<TreeTest>.DeleteNodeInTree(trees[i], nodeId);
                }
            }
            else
            {
                Assert.True(false);
            }
        }
    }
    public class TreeTest : absTree<TreeTest>
    {

    }
}
