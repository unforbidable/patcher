
Member types
Built in member types are: byte, sbyte, short, ushort, int, uint, float and string (including variations of string below).
These types can be also used as arrays with fixed length, e.g uint[4] or variable length e.g. byte[].
Arrays of variable length are used to load content of the whole field.
Enums can be used as member types.

Enums
Enums consist of enum members and each member must be the same underlying type.
Permitted enum types are as follows:  byte, sbyte, short, ushort, int, uint
Enums are always defined in separate files and can be used as member types.

Structures
Structures are made from members. Each members needs a name and type.
Structures may have a name. Structures without a name are called anonymous and these will not be generated.
Properties of anonymous structures will be geenerated in the parent scope (e.g. record or field group)
Structures can be defined in separate files for reusability or inline where they are needed.

Fields
Fields directly define the content of form records. Fields can be represented by a member (of any built in type or enums), a structure or a field group.
Fields have a key (eg. EDID), with the exeption of fields represented by a field group (in this case the keys of the child fields are used).
Fields can may be defined in separate files and included anywhere to avoid redundancy. When a field is included, it is possible (and other required)
to provide key and name.
Fields can be used a list.
Fields can be declared as virtual in which case there is no underlying data and only a property is generated.
Fields can be declared as hidden in which case no property is generated for them.
Properties of virtual fields can access one or more hidden fields (via so called adapters).

Field Groups
Field groups are sort of like structures, but they contain fields (basically, memebers with keys).
Field groups will be generated sort of like structures.

Records
Records consist of fields. 
Every records has a unique form type (e.g. KYWD) which is inferred from the name of the file in which they are defined.
Records also have a user friendly name (e.g. Keyword).
