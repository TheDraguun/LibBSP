using System;
using System.Collections.Generic;

namespace LibBSP {
	/// <summary>
	/// Contains all data needed for a node in a BSP tree.
	/// </summary>
	public struct Node {

		public int plane { get; private set; }
		public int child1 { get; private set; } // Negative values are valid here. However, the child can never be zero,
		public int child2 { get; private set; } // since that would reference the head node causing an infinite loop.

		/// <summary>
		/// Creates a new <c>Node</c> object from a <c>byte</c> array.
		/// </summary>
		/// <param name="data"><c>byte</c> array to parse</param>
		/// <param name="type">The map type</param>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype</exception>
		public Node(byte[] data, MapType type) : this() {
			if (data == null) {
				throw new ArgumentNullException();
			}
			plane = BitConverter.ToInt32(data, 0); // All formats I've seen use the first 4 bytes as an int, plane index
			switch (type) {
				case MapType.Quake: {
					this.child1 = BitConverter.ToInt16(data, 4);
					this.child2 = BitConverter.ToInt16(data, 6);
					break;
				}
				// These all use the first three ints for planenum and children
				case MapType.SiN:
				case MapType.SoF:
				case MapType.Quake2:
				case MapType.Daikatana:
				case MapType.Nightfire:
				case MapType.Source17:
				case MapType.Source18:
				case MapType.Source19:
				case MapType.Source20:
				case MapType.Source21:
				case MapType.Source22:
				case MapType.Source23:
				case MapType.Source27:
				case MapType.TacticalIntervention:
				case MapType.Vindictus:
				case MapType.DMoMaM:
				case MapType.STEF2:
				case MapType.MOHAA:
				case MapType.STEF2Demo:
				case MapType.Raven:
				case MapType.Quake3:
				case MapType.FAKK:
				case MapType.CoD: {
					this.child1 = BitConverter.ToInt32(data, 4);
					this.child2 = BitConverter.ToInt32(data, 8);
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Node class.");
				}
			}
		}

		/// <summary>
		/// Factory method to parse a <c>byte</c> array into a <c>List</c> of <c>Node</c> objects.
		/// </summary>
		/// <param name="data">The data to parse</param>
		/// <param name="type">The map type</param>
		/// <returns>A <c>List</c> of <c>Node</c> objects</returns>
		/// <exception cref="ArgumentNullException"><paramref name="data" /> was null</exception>
		/// <exception cref="ArgumentException">This structure is not implemented for the given maptype</exception>
		public static List<Node> LumpFactory(byte[] data, MapType type) {
			if (data == null) {
				throw new ArgumentNullException();
			}
			int structLength = 0;
			switch (type) {
				case MapType.Quake: {
					structLength = 24;
					break;
				}
				case MapType.Quake2:
				case MapType.SiN:
				case MapType.SoF:
				case MapType.Daikatana: {
					structLength = 28;
					break;
				}
				case MapType.Source17:
				case MapType.Source18:
				case MapType.Source19:
				case MapType.Source20:
				case MapType.Source21:
				case MapType.Source22:
				case MapType.Source23:
				case MapType.Source27:
				case MapType.TacticalIntervention:
				case MapType.DMoMaM: {
					structLength = 32;
					break;
				}
				case MapType.Vindictus: {
					structLength = 48;
					break;
				}
				case MapType.Quake3:
				case MapType.FAKK:
				case MapType.CoD:
				case MapType.STEF2:
				case MapType.STEF2Demo:
				case MapType.MOHAA:
				case MapType.Raven:
				case MapType.Nightfire: {
					structLength = 36;
					break;
				}
				default: {
					throw new ArgumentException("Map type " + type + " isn't supported by the Node lump factory.");
				}
			}
			int offset = 0;
			List<Node> lump = new List<Node>(data.Length / structLength);
			byte[] bytes = new byte[structLength];
			for (int i = 0; i < data.Length / structLength; ++i) {
				Array.Copy(data, (i * structLength), bytes, 0, structLength);
				lump.Add(new Node(bytes, type));
				offset += structLength;
			}
			return lump;
		}
	}
}