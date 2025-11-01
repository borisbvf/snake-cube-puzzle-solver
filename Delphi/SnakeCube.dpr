program SnakeCube;

{$APPTYPE CONSOLE}

uses
  System.SysUtils, System.Generics.Collections, DateUtils;

type
  // Type for storing a move of a solution
  TMove = record
    Axis: Integer;
    Direction: Integer;
    Value: Integer;
  end;

  TCubePoint = array[0..2] of Integer;

const
  // The lengths of the snake cube puzzle
  LengthValues: array of Integer = [3, 2, 3, 2, 2, 4, 2, 3, 2, 3, 2, 3, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 2, 2, 2, 2, 2, 3, 4, 2, 2, 2, 4, 2, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 2];

  // The dimension of the cube
  CubeSize = 4;

var
  // State array
  State: array[0..CubeSize - 1, 0..CubeSize - 1, 0..CubeSize - 1] of Boolean;

  // Place for storing a solution
  Path: TList<TMove>;

  I, J, K: Integer;
  Start, Finish: TDateTime;
  Res: Boolean;
  Move: TMove;
  StartPoint: TCubePoint;
  S: string;

function GetResultPoint(StartPoint: TCubePoint; Axis: Integer; Sign: Integer; Size: Integer): TCubePoint;
begin
  Result[0] := StartPoint[0];
  Result[1] := StartPoint[1];
  Result[2] := StartPoint[2];
  case Axis of
    0: Result[0] := Result[0] + Size * Sign;
    1: Result[1] := Result[1] + Size * Sign;
    2: Result[2] := Result[2] + Size * Sign;
  end;
end;

procedure SetState(StartPoint: TCubePoint; Axis: Integer; Sign: Integer; Size: Integer; Value: boolean);
var
  Point: TCubePoint;
  I: Integer;
begin
  for I := 1 to Size do
  begin
    Point := GetResultPoint(StartPoint, Axis, Sign, I);
    State[Point[0], Point[1], Point[2]] := Value;
  end;
end;

function CheckState(StartPoint: TCubePoint; Axis: Integer; Sign: Integer; Size: Integer): Boolean;
var
  Point: TCubePoint;
  I: Integer;
begin
  Result := True;
  for I := 1 to Size do
  begin
    Point := GetResultPoint(StartPoint, Axis, Sign, I);
    if State[Point[0], Point[1], Point[2]] then
    begin
      Result := False;
      Exit;
    end;
  end;
end;

// Recursive function with backtracking
function FindSolution(StartPoint: TCubePoint; Axis: Integer; Sign: Integer; LengthIndex: Integer): Boolean;
var
  Size: Integer;
  Current: TCubePoint;
  Move: TMove;
  I, J: Integer;
begin
  Size := LengthValues[LengthIndex] - 1;

  Current := GetResultPoint(StartPoint, Axis, Sign, Size);

  // Check if new point is out of the cube dimensions
  if (Current[Axis] < 0) or (Current[Axis] >= CubeSize) then
  begin
    Result := False;
    Exit;
  end;

  // Check if the new point crosses earlier added
  if not CheckState(StartPoint, Axis, Sign, Size) then
  begin
    Result := False;
    Exit;
  end;

  // Save the path
  Move.Axis := Axis;
  Move.Direction := Sign;
  Move.Value := Size + 1;
  Path.Add(Move);

  // Check if this is the last move
  if LengthIndex = Length(LengthValues) - 1 then
  begin
    Result := True;
    Exit;
  end;

  // Set state
  SetState(StartPoint, Axis, Sign, Size, True);

  // Try recursive next ways from here
  for I := 0 to 2 do
    if I <> Axis then
      for J := -1 to 1 do
        if (J <> 0) and FindSolution(Current, I, J, LengthIndex + 1) then
        begin
          Result := True;
          Exit;
        end;

  // If there is no way to the result, roll back the path
  Path.Delete(Path.Count - 1);

  // Get state back
  SetState(StartPoint, Axis, Sign, Size, False);

  Result := False;
end;

begin
  try
    // Initialize state array
    for I := 0 to CubeSize - 1 do
      for J := 0 to CubeSize - 1 do
        for K := 0 to CubeSize - 1 do
          State[I, J, K] := False;


    // A place for storing the solution
    Path := TList<TMove>.Create();
    try
      State[0, 0, 0] := true;
      Start := Now;

      // Run a search
      StartPoint[0] := 0;
      StartPoint[1] := 0;
      StartPoint[2] := 0;
      Res := FindSolution(StartPoint, 0, 1, 0);

      Finish := Now;
      Writeln(Format('Finding a solution took %0:f seconds.', [MilliSecondsBetween(Start, Finish) / 1000]));

      // Show a solution
      for Move in Path do
      begin
        S := '';
        if (Move.Axis = 0) and (Move.Direction = 1) then
          S := 'To the right'
        else if (Move.Axis = 0) and (Move.Direction = -1) then
          S := 'To the left'
        else if (Move.Axis = 1) and (Move.Direction = 1) then
          S := 'Up'
        else if (Move.Axis = 1) and (Move.Direction = -1) then
          S := 'Down'
        else if (Move.Axis = 2) and (Move.Direction = 1) then
          S := 'Forward'
        else if (Move.Axis = 2) and (Move.Direction = -1) then
          S := 'Backward';

        Writeln(Format('%0:s %1:d', [S, Move.Value]));
      end;

      Readln;
    finally
      Path.Free;
    end;
  except
    ExitCode := 1;
  end;

end.
