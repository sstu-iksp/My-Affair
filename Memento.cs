using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Construct
{
	//																							В РАЗРАБОТКЕ !!!
	/*
	// Создатель содержит некоторое важное состояние, которое может со временем
	// меняться. Он также объявляет метод сохранения состояния внутри снимка и
	// метод восстановления состояния из него.
	class Originator
	{
		// Для удобства состояние создателя хранится внутри одной переменной.
		private string _state;
		
		public Originator(string state)
		{
			this._state = state;
			Console.WriteLine("Originator: My initial state is: " + state);
		}
		
		// Бизнес-логика Создателя может повлиять на его внутреннее состояние.
		// Поэтому клиент должен выполнить резервное копирование состояния с
		// помощью метода save перед запуском методов бизнес-логики.
		public void DoSomething()
		{
			Console.WriteLine("Originator: I'm doing something important.");
			this._state = this.GenerateRandomString(30);
			Console.WriteLine("Originator: and my state has changed to: " + _state);
		}
		
		private string GenerateRandomString(int length = 10)
		{
			const string allowedSymbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
			string result = string.Empty;
			
			while (length > 0)
			{
				result += allowedSymbols[new Random().Next(0, allowedSymbols.Length)];
				
				Thread.Sleep(12);
				
				length--;
			}
		
			return result;
		}
		
		// Сохраняет текущее состояние внутри снимка.
		public IMemento Save()
		{
			return new ConcreteMemento(this._state);
		}
		
		// Восстанавливает состояние Создателя из объекта снимка.
		public void Restore(IMemento memento)
		{
			if (!(memento is ConcreteMemento))
			{
				throw new Exception("Unknown memento class " + memento.ToString());
			}
			
			this._state = memento.GetState();
			Console.Write("Originator: My state has changed to: " + _state);
		}
	}
	
	// Интерфейс Снимка предоставляет способ извлечения метаданных снимка, таких
	// как дата создания или название. Однако он не раскрывает состояние создателя.
	public interface IMemento
	{
		string GetName();
		string GetState();
		DateTime GetDate();
	}
	
	// Конкретный снимок содержит инфраструктуру для хранения состояния создателя.
	class ConcreteMemento : IMemento
	{
		string _state;
		
		DateTime _date;
		
		public ConcreteMemento(string state)
		{
		    this._state = state;
		    this._date = DateTime.Now;
		}
		
		// Создатель использует этот метод, когда восстанавливает своё
		// состояние.
		public string GetState()
		{
		    return this._state;
		}
		
		// Остальные методы используются Опекуном для отображения метаданных.
		public string GetName()
		{
		    return this._date + "/" + this._state.Substring(0, 9) +  "...";
		}
		
		public DateTime GetDate()
		{
		    return this._date;
		}
	}
	
	// Опекун не зависит от класса Конкретного Снимка. Таким образом, он не
	// имеет доступа к состоянию создателя, хранящемуся внутри снимка. Он
	// работает со всеми снимками через базовый интерфейс Снимка.
	class Caretaker
	{
		// Коллекция хранящая снимки
		List<IMemento> _mementos = new List<IMemento>();
		Originator _originator = null;
		
		public Caretaker(Originator originator)
		{
			this._originator = originator;
		}
		// Метод делающий бекап текущего снимка
		public void Backup()
		{
			Console.WriteLine("\nCaretaker: Saving Originator's state...");
			this._mementos.Add(this._originator.Save());
		}
		// Метод окатывающий изменения
		public void Undo()
		{
			// Проеверяем не находимся ли мы в последнем снимке
			if (this._mementos.Count == 0)
			{
			    return;
			}
			// Изменяем текущий снимок на последней из колекции
			var memento = this._mementos.Last();
			this._mementos.Remove(memento);
			
			Console.WriteLine("Caretaker: Restoring state to: " + memento.GetName());
			
			try
			{
				// Востанавливаем данные из снимка
			    this._originator.Restore(memento);
			}
			catch (Exception)
			{
			    this.Undo();
			}
		}
		
		public void ShowHistory()
		{
			Console.WriteLine("Caretaker: Here's the list of mementos:");
			
			foreach (var memento in this._mementos)
			{
			    Console.WriteLine(memento.GetName());
			}
		}
	}
	
	*/
	
}




