//Nombre: David André Rodríguez Cano
//Carné: 1164619



using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using static BinaryTree;

//Clase para los datos de Persona
public class Person
{
    public string Name { get; set; }
    public string DPI { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; }

    public override string ToString()
    {
        return $"{Name} (DPI: {DPI})";
    }
}
//Clase para los nodos del árbol 
public class Node
{
    public Person Data { get; set; }
    public Node Left { get; set; }
    public Node Right { get; set; }

    public Node(Person person)
    {
        Data = person;
        Left = null;
        Right = null;
    }
}

//Clase para el arbol binario 
public class BinaryTree
{
    private Node root;

    public BinaryTree()
    {
        root = null;
        personsList = new List<Person>();
    }
    //Inserción de datos
    public void Insert(Person person)
    {
        root = InsertRec(root, person);
        if (root != null) // Si se insertó correctamente
        {
            personsList.Add(person); // Agregar a la lista
        }
    }

    private Node InsertRec(Node root, Person person)
    {
        if (root == null)
        {
            root = new Node(person);
            Console.WriteLine("Inserción exitosa: " + person.Name);
        }
        else
        {
            int compareResult = string.Compare(person.Name, root.Data.Name, StringComparison.OrdinalIgnoreCase);
            if (compareResult < 0)
            {
                root.Left = InsertRec(root.Left, person);
            }
            else if (compareResult > 0)
            {
                root.Right = InsertRec(root.Right, person);
            }
            else
            {
                Console.WriteLine("Persona con el mismo nombre ya existe: " + person.Name);
            }
        }
        return root;
    }
    //Actualización de datos
    public void Update(string name, Person updatedPerson)
    {
        root = UpdateRec(root, name, updatedPerson);
    }

    private Node UpdateRec(Node root, string name, Person updatedPerson)
    {
        if (root == null)
        {
            Console.WriteLine("No se encontró la persona a actualizar: " + name);
        }
        else
        {
            int compareResult = string.Compare(name, root.Data.Name, StringComparison.OrdinalIgnoreCase);
            if (compareResult == 0)
            {
                root.Data = updatedPerson;
                Console.WriteLine("Actualización exitosa para: " + name);
            }
            else if (compareResult < 0)
            {
                root.Left = UpdateRec(root.Left, name, updatedPerson);
            }
            else
            {
                root.Right = UpdateRec(root.Right, name, updatedPerson);
            }
        }
        return root;
    }



    public void InOrderTraversal()
    {
        Console.WriteLine("Listado de Personas:");
        InOrderTraversalRec(root);
    }

    private void InOrderTraversalRec(Node root)
    {
        if (root != null)
        {
            InOrderTraversalRec(root.Left);
            Console.WriteLine(root.Data);
            InOrderTraversalRec(root.Right);
        }
    }
    //Eliminación de datos
    public void Delete(string nameToDelete)
    {
        root = DeleteRec(root, nameToDelete);
        if (root != null) // Si se eliminó correctamente
        {
            personsList.RemoveAll(p => p.Name.Equals(nameToDelete, StringComparison.OrdinalIgnoreCase)); // Eliminar de la lista
        }
    }

    private Node DeleteRec(Node root, string nameToDelete)
    {
        if (root == null)
        {
            // No se encontró la persona a eliminar
            Console.WriteLine("No se encontró la persona a eliminar: " + nameToDelete);
            return root;
        }

        int compareResult = string.Compare(nameToDelete, root.Data.Name, StringComparison.OrdinalIgnoreCase);
        if (compareResult < 0)
        {
            root.Left = DeleteRec(root.Left, nameToDelete);
        }
        else if (compareResult > 0)
        {
            root.Right = DeleteRec(root.Right, nameToDelete);
        }
        else
        {
            // Se encontró la persona a eliminar
            Console.WriteLine("Eliminación exitosa: " + nameToDelete);

            // Caso 1: No tiene hijos o solo un hijo
            if (root.Left == null)
            {
                return root.Right;
            }
            else if (root.Right == null)
            {
                return root.Left;
            }

            // Caso 2: Tiene dos hijos, se encuentra el sucesor inmediato
            root.Data = FindMinValue(root.Right);

            // Elimina el sucesor inmediato
            root.Right = DeleteRec(root.Right, root.Data.Name);
        }
        return root;
    }

    private Person FindMinValue(Node node)
    {
        Person minValue = node.Data;
        while (node.Left != null)
        {
            minValue = node.Left.Data;
            node = node.Left;
        }
        return minValue;
    }
    //Busqueda de datos
    public Person Search(string name)
    {
        return SearchRec(root, name);
    }

    private Person SearchRec(Node root, string name)
    {
        if (root == null)
        {
            // No se encontró la persona
            return null;
        }

        int compareResult = string.Compare(name, root.Data.Name, StringComparison.OrdinalIgnoreCase);
        if (compareResult == 0)
        {
            // Se encontró la persona
            return root.Data;
        }
        else if (compareResult < 0)
        {
            // La persona podría estar en el subárbol izquierdo
            return SearchRec(root.Left, name);
        }
        else
        {
            // La persona podría estar en el subárbol derecho
            return SearchRec(root.Right, name);
        }
    }
    //Clase para los datos de Bitacora
    public class BitacoraEntry
    {
        public string Name { get; set; }
        public string DPI { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }

        public BitacoraEntry(Person person)
        {
            Name = person.Name;
            DPI = person.DPI;
            DateOfBirth = person.DateOfBirth;
            Address = person.Address;
        }

        public BitacoraEntry() { }
    }

  //Listado de personas despues del proceso de lectura (Personas despues de la eliminación)
    private List<Person> personsList;
    
    public List<Person> GetPersonsList()
    {
        return personsList; // Obtener la lista de personas
    }


}

class Program
{
    static void Main()
    {
        BinaryTree binaryTree = new BinaryTree();

        // Ruta del archivo de entrada (datos.txt) que contiene las acciones y datos
        string inputFilePath = @"D:\Cosas\Clases\2023\Estructura de datos\txt\datos.txt"; 
        string[] lines = File.ReadAllLines(inputFilePath);
        //Lectura de datos del archivo
        foreach (string line in lines)
        {
            string[] parts = line.Split(';');
            if (parts.Length == 2)
            {
                string action = parts[0].Trim();
                string data = parts[1].Trim();
                //Acciones de datos
                switch (action)
                {
                    case "INSERT":
                        var personData = JsonConvert.DeserializeObject<Person>(data);
                        binaryTree.Insert(personData);
                        break;

                    case "PATCH":
                        var updatedPersonData = JsonConvert.DeserializeObject<Person>(data);
                        binaryTree.Update(updatedPersonData.Name, updatedPersonData);
                        break;
                        

                    case "DELETE":
                        var deleteData = JsonConvert.DeserializeObject<Person>(data);
                        binaryTree.Delete(deleteData.Name);
                        break;

                    default:
                        Console.WriteLine("Acción no reconocida: " + action);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Formato incorrecto en línea: " + line);
            }
        }
        Console.WriteLine("------------------------------------------");
        Console.WriteLine("Presiona cualquier tecla para Continuar...");


        //Escritura de las personas despues de las operaciones
        Console.ReadKey();
        Console.Clear();
        Console.WriteLine("\nListado de Personas después de las operaciones:");
        List<Person> personsAfterOperations = binaryTree.GetPersonsList();
        foreach (var person in personsAfterOperations)
        {
            Console.WriteLine(person);
        }
        Console.WriteLine("------------------------------------------");
        Console.WriteLine("Presiona cualquier tecla para Continuar...");
        Console.ReadKey();
        Console.Clear();

        //Creación de archivo con el listado de personas aun existente
        // Definir la ruta del archivo "Listado.txt" en la misma ubicación que el archivo "datos.txt"
        string listadoFilePath = Path.Combine(Path.GetDirectoryName(inputFilePath), "Listado.txt");

        // Limpiar el archivo Listado.txt si existe
        if (File.Exists(listadoFilePath))
        {
            File.Delete(listadoFilePath);
        }

        // Guardar el listado de personas que quedan en un archivo llamado "Listado.txt"
        using (StreamWriter writer = new StreamWriter(listadoFilePath))
        {
            writer.WriteLine("Listado de Personas que Quedan Después de las Operaciones:");
            List<Person> personsAfterOperations2 = binaryTree.GetPersonsList();
            foreach (var person in personsAfterOperations)
            {
                writer.WriteLine(person.ToString());
            }
        }

        Console.WriteLine("Listado de Personas que Quedan guardado en Listado.txt.");
        Console.WriteLine("------------------------------------------");
        Console.WriteLine("Presiona cualquier tecla para Continuar...");
        Console.ReadKey();
        Console.Clear();


        //Busqueda de personas en console (Con el nombre de la persona)
        Console.WriteLine("Ingresa el nombre a buscar:");
        string nameToSearch = Console.ReadLine();
        Person foundPerson = binaryTree.Search(nameToSearch);
        if (foundPerson != null)
        {
            Console.Clear();
            Console.WriteLine("Persona encontrada:");
            Console.WriteLine("Nombre: " + foundPerson.Name);
            Console.WriteLine("DPI: " + foundPerson.DPI);
            Console.WriteLine("Fecha de Nacimiento: " + foundPerson.DateOfBirth.ToShortDateString());
            Console.WriteLine("Dirección: " + foundPerson.Address);

            // Crear una entrada de bitácora
            BitacoraEntry bitacoraEntry = new BitacoraEntry(foundPerson);

            // Convertir la entrada de bitácora a JSON
            string bitacoraJson = JsonConvert.SerializeObject(bitacoraEntry);

            // Ruta del archivo de bitácora (bitacora.jsonl) en la misma ubicación que datos.txt
            string bitacoraFilePath = Path.Combine(Path.GetDirectoryName(inputFilePath), "bitacora.jsonl");

            File.AppendAllText(bitacoraFilePath, bitacoraJson + Environment.NewLine);

            Console.WriteLine("----> Entrada de bitácora registrada.");
        }
        else
        {
            Console.WriteLine("No se encontró la persona.");
        }

        Console.WriteLine("------------------------------------------");
        Console.WriteLine("Presiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}