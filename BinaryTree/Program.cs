using System;

namespace BinaryTree
{
	public class BinaryTree<T> where T : IComparable<T>
	{
		public class Node
		{
			public T Data;
			public Node Left;
			public Node Right;
		}

		public Node Root;

		public void Add(T t)
		{
			Node node = new Node
			{
				Data = t
			};

			if (Root == null)
			{
				Root = node;
				return;
			}

			Node current = Root;
			Node previous = null;
			while (current != null)
			{
				previous = current;

				int c = t.CompareTo(current.Data);

				if (c > 0)
				{
					current = current.Right;
				}
				else
				{
					current = current.Left;
				}
			}

			if (t.CompareTo(previous.Data) > 0)
			{
				previous.Right = node;
			}
			else
			{
				previous.Left = node;
			}
		}

		public bool Delete(T t)
		{
			// Find t in the tree
			Node current = Root;
			Node previous = null;
			bool left = false;
			while (current != null)
			{
				int c = t.CompareTo(current.Data);
				if (c == 0)
				{
					break;
				}

				previous = current;
				if (c > 0)
				{
					current = current.Right;
					left = false;
				}
				else
				{
					current = current.Left;
					left = true;
				}
			}

			// t is not in the tree, so it cannot be deleted - failure
			if (current == null)
			{
				return false;
			}

			// t is the only item in the tree
			if (previous == null && current.Left == null && current.Right == null)
			{
				Root = null;
				return true;
			}

			// If t has no children, set the pointer from the parent to t to null
			if (current.Left == null && current.Right == null)
			{
				if (left) previous.Left = null;
				else previous.Right = null;

				return true;
			}
			// If t only has one child, set the pointer from the parent to t's child
			else if (current.Right == null)
			{
				if (previous == null)
				{
					Root = current.Left;
				}
				else if (left) previous.Left = current.Left;
				else previous.Right = current.Left;

				return true;
			}
			else if (current.Left == null)
			{
				if (previous == null)
				{
					Root = current.Right;
				}
				else if (left) previous.Left = current.Right;
				else previous.Right = current.Right;

				return true;
			}
			// Node has two children
			else
			{
				Node min = current.Right;
				Node p = current;
				while (min.Left != null)
				{
					p = min;
					min = min.Left;
				}

				current.Data = min.Data;
				if (p == current)
				{
					current.Right = min.Right;
				}
				else
				{
					p.Left = null;
				}

				return true;
			}

			return false;
		}

		public Node FindNode(T t)
		{
			Node current = Root;
			Node previous = null;
			while (current != null)
			{
				previous = current;

				int c = t.CompareTo(current.Data);

				if (c == 0)
				{
					return current;
				}
				else if (c > 0)
				{
					current = current.Right;
				}
				else
				{
					current = current.Left;
				}
			}

			return previous;
		}

		public int Count
		{
			get
			{
				if (Root == null)
				{
					return 0;
				}

				return GetSubtreeCount(Root);
			}
		}

		public bool Valid
		{
			get
			{
				if (Root == null)
				{
					return true;
				}

				return SubtreeValid(Root);
			}
		}

		private bool SubtreeValid(Node subtree)
		{
			bool result = true;

			if (subtree.Left != null)
			{
				result &= subtree.Left.Data.CompareTo(subtree.Data) < 0 && SubtreeValid(subtree.Left);
			}
			if (subtree.Right != null)
			{
				result &= subtree.Right.Data.CompareTo(subtree.Data) > 0 && SubtreeValid(subtree.Right);
			}

			return result;
		}

		private int GetSubtreeCount(Node subtree)
		{
			int count = 0;

			if (subtree.Left != null)
			{
				count += GetSubtreeCount(subtree.Left);
			}
			if (subtree.Right != null)
			{
				count += GetSubtreeCount(subtree.Right);
			}

			return count + 1;
		}

		public bool Contains(T t)
		{
			Node node = FindNode(t);
			if (node == null)
			{
				return false;
			}
			return node.Data.CompareTo(t) == 0;
		}

		public void PrintTree()
		{
			if (Root == null)
			{
				return;
			}

			PrintSubtree(Root, true, "");
		}

		private void PrintSubtree(Node node, bool leaf, string indent)
		{
			Console.Write(indent);
			if (leaf)
			{
				Console.Write("└");
				indent += " ";
			}
			else
			{
				Console.Write("├");
				indent += "│";
			}

			Console.WriteLine(node.Data.ToString());

			if (node.Left != null)
			{
				PrintSubtree(node.Left, node.Right == null, indent);
			}
			if (node.Right != null)
			{
				PrintSubtree(node.Right, true, indent);
			}
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			Interactive();
		}

		class InteractiveAction
		{
			public string Name;
			public Action Action;

			public InteractiveAction(string description, Action action)
			{
				Name = description;
				Action = action;
			}
		}

		static int GetInt(Predicate<int> condition)
		{
			while (true)
			{
				string input = Console.ReadLine();
				if (int.TryParse(input, out int x) && condition(x))
				{
					return x;
				}
				else
				{
					Console.WriteLine("Invalid");
				}
			}
		}

		static void Interactive()
		{
			var bt = new BinaryTree<int>();

			InteractiveAction[] actions =
			{
				new InteractiveAction("help", null),
				new InteractiveAction("clear", () =>
				{
					bt = new BinaryTree<int>();
				}),
				new InteractiveAction("print", () =>
				{
					bt.PrintTree();
				}),
				new InteractiveAction("random", () =>
				{
					Console.WriteLine("Enter number of items");
					int n = GetInt(x => x > 0);

					Console.WriteLine("Enter minimum value");
					int min = GetInt(_ => true);

					Console.WriteLine("Enter maximum value");
					int max = GetInt(x => x > min && (x - min + 1) >= n);

					bt = new BinaryTree<int>();

					Random rng = new Random();
					for (int i = 0; i < n; i++)
					{
						int x;
						do
						{
							x = rng.Next(min, max + 1);
						} while (bt.Contains(x));

						bt.Add(x);
					}

					bt.PrintTree();
				}),
				new InteractiveAction("add", () =>
				{
					Console.WriteLine("Enter item to add");
					int x = GetInt(_ => true);

					bt.Add(x);

					bt.PrintTree();
				}),
				new InteractiveAction("delete", () =>
				{
					Console.WriteLine("Enter item to delete");
					int x = GetInt(_ => true);

					if (bt.Delete(x))
					{
						Console.WriteLine("Delete successful");
					}
					else
					{
						Console.WriteLine("Delete failed");
					}

					bt.PrintTree();
				})
			};

			actions[0].Action = () =>
			{
				Console.WriteLine("Command list:");
				foreach (var ia in actions)
				{
					Console.WriteLine(ia.Name);
				}
			};

			while (true)
			{
				string input = Console.ReadLine();
				InteractiveAction ia = Array.Find(actions, a => a.Name == input);
				if (ia == null)
				{
					Console.WriteLine("Invalid command. For more info, type help");
				}
				else
				{
					ia.Action();
				}
			}
		}

		static void Test<T>(string name, Func<T> test, T expected) where T : IEquatable<T>
		{
			T result = test();
			if (result.Equals(expected))
			{
				Console.WriteLine($"Pass: {name}");
			}
			else
			{
				Console.WriteLine($"Fail: {name}\nExpected: {expected}\nResult: {result}");
			}
		}

		static void RunTests()
		{
			Test("Delete from empty tree", () =>
			{
				var bt = new BinaryTree<int>();
				return bt.Delete(4) == false && bt.Count == 0;
			}, true);

			Test("Delete non-existant item", () =>
			{
				var bt = new BinaryTree<int>();

				bt.Add(5);

				bool result = true;
				result &= bt.Contains(5);
				result &= bt.Delete(3) == false;
				result &= bt.Count == 1;
				result &= bt.Contains(5) == true;
				result &= bt.Valid;
				return result;
			}, true);

			Test("Delete only item in tree", () =>
			{
				var bt = new BinaryTree<int>();

				bt.Add(5);

				return bt.Contains(5) && bt.Delete(5) == true && bt.Count == 0 && !bt.Contains(5) && bt.Valid;
			}, true);

			Test("Delete childless node deep in tree", () =>
			{
				var bt = GetTestTree1();

				bool result = true;
				result &= bt.Contains(4);
				result &= bt.Delete(4);
				result &= bt.Count == 9;
				result &= !bt.Contains(4);
				result &= bt.Valid;
				return result;
			}, true);

			Test("Delete node with only left child", () =>
			{
				var bt = GetTestTree1();

				return
					bt.FindNode(5).Right.Data == 7 &&
					bt.Delete(7) == true &&
					bt.Count == 9
					&& bt.FindNode(5).Right.Data == 6
					&& bt.Valid;
			}, true);

			Test("Delete node with only right child", () =>
			{
				var bt = GetTestTree1();

				return
					bt.FindNode(8).Right.Data == 10 &&
					bt.Delete(10) == true &&
					bt.Count == 9
					&& bt.FindNode(8).Right.Data == 11
					&& bt.Valid;
			}, true);

			Test("Delete root node with only left child", () =>
			{
				var bt = new BinaryTree<int>();

				bt.Add(4);
				bt.Add(3);

				return
					bt.Delete(4) &&
					bt.Count == 1
					&& bt.Root.Data == 3
					&& bt.Valid;
			}, true);

			Test("Delete root node with only right child", () =>
			{
				var bt = new BinaryTree<int>();

				bt.Add(4);
				bt.Add(5);

				return
					bt.Delete(4) &&
					bt.Count == 1
					&& bt.Root.Data == 5
					&& bt.Valid;
			}, true);

			Test("Delete node with 2 children", () =>
			{
				var bt = GetTestTree1();

				bool result = true;
				result &= bt.Delete(5);
				result &= bt.Count == 9;
				result &= bt.Valid;

				//bt.PrintTree();

				return result;
			}, true);

			Test("Delete root", () =>
			{
				var bt = GetTestTree1();

				bool result = true;
				result &= bt.Delete(8);
				result &= bt.Count == 9;
				result &= bt.Valid;

				//bt.PrintTree();

				return result;
			}, true);

			Test("Tree validation", () =>
			{
				var bt = new BinaryTree<int>();
				bt.Add(5);
				bt.FindNode(5).Left = new BinaryTree<int>.Node { Data = 6 };

				return bt.Valid == false;
			}, true);
		}

		static BinaryTree<int> GetTestTree1()
		{
			var bt = new BinaryTree<int>();

			bt.Add(8);
			bt.Add(1);
			bt.Add(10);
			bt.Add(11);
			bt.Add(5);
			bt.Add(3);
			bt.Add(2);
			bt.Add(4);
			bt.Add(7);
			bt.Add(6);

			//bt.PrintTree();
			/*
			└8
			 ├1
			 │└5
			 │ ├3
			 │ │├2
			 │ │└4
			 │ └7
			 │  └6
			 └10
			  └11
			*/

			return bt;
		}
	}
}
