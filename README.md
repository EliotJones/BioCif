# Bio CIF #

<img src="https://raw.githubusercontent.com/EliotJones/BioCif/master/icon.png" width="128px"/>

BioCif is a small C# library designed to parse the [Crystallographic Information File](https://www.iucr.org/resources/cif) (CIF) format, the standard for information interchange in crystallography. It is designed to be fast and easy-to-use.

It provides access to both Tokenization and Parsing of CIF formats for both version 1.1 and version 2.0 as well as convenience wrappers for an API for the [Protein Data Bank](https://www.rcsb.org/) (PDB) data. The PDB hosts CIF format data (PDBx/mmCIF - Macro-molecular CIF) for protein structure.

## Usage ##

To access the raw stream of tokens:

    using BioCif.Core.Tokenization;
    using BioCif.Core.Tokens;

    using (var fileStream = File.Open(@"C:\path\to\data.cif"))
    using (var streamReader = new StreamReader(fileStream))
    {
        foreach (Token token in CifTokenizer.Tokenize(streamReader))
        {
            Console.WriteLine(token.TokenType);
        }
    }

To access the parsed CIF structure:

    using (var fileStream = File.Open(@"C:\path\to\data.cif"))
    {
        Cif cif = CifParser.Parse(fileStream);

        DataBlock block = cif.DataBlocks[0];
        Console.WriteLine($"Block name: {block.Name}");

        foreach (IDataBlockMember member in block.Members)
        {
            // ...
        }
    }

To access a parsed PDBx/mmCIF:

    Pdbx pdbx = PdbxParser.ParseFile(@"C:\path\to\mypdbx.cif");
    PdbxDataBlock block = pdbx.First;
    List<AuditAuthor> auditAuthors = block.AuditAuthors;


## Notes ##

Defined terms from the CIF specification: 

+ data file - information relating to an experiment
+ dictionary file - contains information about data names
+ data name (AKA Tag): identifies the content of a data value
+ data value: string representing a value of any type.
+ data item: data name + data value

Notes on structures within a CIF file:

    data block : highest level of cif file
      data_<block name>
      [data items or save frames]

    save frame: partitionaed collection of data items
      save_<frame code>
      [data items]
      save_   # Terminates the save frame
      ^ only used in dictionary files

## Useful Links ##

+ Dictionary for PDBx/mmCIF data names: http://mmcif.wwpdb.org/dictionaries/mmcif_pdbx_v50.dic/Index/
+ CIF Version 1.1 specification: https://www.iucr.org/resources/cif/spec/version1.1/cifsyntax
+ Search PDBx structures in the PDB: https://www.rcsb.org/#Category-search
+ Existing C# tools for CIF format among others: https://github.com/mindleaving/genome-tools
+ CIF on Wikipedia: https://en.wikipedia.org/wiki/Crystallographic_Information_File
+ Crystallography Open Database of non-mmCIF CIF files:http://www.crystallography.net/cod/index.php

## Status ##

Early stage/incomplete/unmaintained.