using System;
/* Battleships Game - Laivų mūšis

Sukurtas laivų mūšio žaidimas, kuomet laivai.txt faile yra pateiktos 10-ties laivų koordinatės (keturi vienviečiai, trys dviviečiai, du triviečiai ir vienas keturvietis).
Laivų koordinatės sustatytos 10 ant 10 langelių lentelėje, kurios x ašyje yra išskaidytas žodis respublika, o y ašyje yra pažymėti skaičiai nuo 1 iki 10.
Koordinatės tekstiniame faile žymimos nuo min iki max reikšmių t.y. jeigu tarkim žymimas keturvietis laivas, jo koordinatės bus s 9 b 9. Tai reiškia, kad laivas stovi ant s 9 p 9 u 9 b 9 langelių.
Jų stovėjimas yra žymimas tekstiniame faile, kurio pilnas path yra paduodamas programai.
Žaidimą žaidžia vienas žmogus, pradėdamas vesti koordinates kur jo manymu stovi laivas (pvz. r 1) - piriausia eina raide, tarpas ir skaičius.
Jei žmogus pataiko į laivą jis pamato žodį Pataikėte ir langelis pasižymi X. Kai laivas yra pilnai nušautas, žaidėjas turi pamatyti sakinį Sunaikinote laivą!
Jeigu žaidėjas prašauna, išvysta žodį Nepataikėte ir langelis pasižymi *.
Žaidimas tęsiasi tol kol yra nušaunami visi 10 laivų.
Žaidimas buvo kuriamas pagal lietuviškas taisykles.*/

namespace Battleships_Game_Samanta_0510
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Board board = new Board("C:\\Users\\316794\\Desktop\\laivai.txt", 1, 4, "respublika"); //paduodamas txt failo path, kuris nusako kur stovi laivai. Skaičiai žymi min ir max koordinatės reikšmę.
            board.startGame();
        }
    }
}
