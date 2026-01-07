Imports IntList = System.Collections.Generic.List(Of Integer)
Imports IntDict = System.Collections.Generic.Dictionary(Of Integer, Integer)

''' <summary>
''' Implements the Partial Digest algorithm for reconstructing a set of points from pairwise distances.
''' 
''' The Partial Digest algorithm (also known as the PDP algorithm) is a computational biology algorithm
''' used to determine the positions of restriction sites along a DNA molecule based on the distances
''' between all pairs of sites.
''' </summary>
Public Module PartialDigestAlgorithm
    ''' <summary>
    ''' Calculates the distances from a given point to all points in a list.
    ''' </summary>
    ''' <param name="refPt">The reference point.</param>
    ''' <param name="points">The list of points to calculate distances to.</param>
    ''' <returns>A list of absolute distances from the reference point to each point in the input list.</returns>
    Private ReadOnly Property Distances(refPt As Integer, points As IntList) As IntList
        Get
            Return New IntList(From p In points Select Math.Abs(p - refPt))
        End Get
    End Property

    ''' <summary>
    ''' Checks if a collection of distances can be removed from the distance multiset.
    ''' </summary>
    ''' <param name="distToRemove">The list of distances to remove.</param>
    ''' <param name="distCounts">The dictionary containing counts of each distance in the multiset.</param>
    ''' <returns>True if all distances can be removed (i.e., they exist in sufficient quantities), False otherwise.</returns>
    Private Function CanRemoveDistances(distToRemove As IntList, distCounts As IntDict) As Boolean
        Dim tempCounts As New IntDict(distCounts)
        For Each dist As Integer In distToRemove
            Dim value As Integer = Nothing
            If Not tempCounts.TryGetValue(dist, value) OrElse value = 0 Then Return False
            tempCounts(dist) -= 1
        Next dist
        Return True
    End Function

    ''' <summary>
    ''' Creates a dictionary that counts occurrences of each distance in a list.
    ''' </summary>
    ''' <param name="distances">The list of distances to count.</param>
    ''' <returns>A dictionary where keys are distance values and values are their counts in the input list.</returns>
    Private ReadOnly Property DistanceCounts(distances As IntList) As IntDict
        Get
            Dim counts As New IntDict
            For Each dist As Integer In distances
                If counts.ContainsKey(dist) Then
                    counts(dist) += 1
                Else
                    counts(dist) = 1
                End If
            Next dist
            Return counts
        End Get
    End Property

    ''' <summary>
    ''' Implements the improved Partial Digest algorithm to find a set of points that produce the given distances.
    ''' </summary>
    ''' <param name="width">The maximum distance in the input (represents the total length).</param>
    ''' <param name="distances">The list of pairwise distances (partial digest).</param>
    ''' <param name="points">The initial set of points (typically containing 0 and width).</param>
    ''' <returns>An ordered array of points that produce the input distances, or an empty array if no solution exists.</returns>
    Public Function FindSolution(width As Integer, distances As IntList, points As IntList) As Integer()
        Dim stackOfTuple As New Stack(Of (IntDict, IntList))

        ' Create initial distance counts
        Dim initialCounts = DistanceCounts(distances)
        stackOfTuple.Push((initialCounts, points))

        Do Until stackOfTuple.Count = 0
            Dim stackElmt As (IntDict, IntList) = stackOfTuple.Pop()
            Dim currCounts As IntDict = stackElmt.Item1
            Dim currPoints As IntList = stackElmt.Item2.ToList() ' Create a copy

            ' Check if we've found a solution (all distances accounted for)
            If currCounts.Values.Sum() = 0 Then
                Return Aggregate x In currPoints Order By x Into ToArray()
            End If

            ' Find the maximum remaining distance
            Dim maxDist As Integer = currCounts.Keys.Max()
            Dim candidate1 As Integer = maxDist
            Dim candidate2 As Integer = width - maxDist

            ' Process first candidate point
            Dim distances1 As IntList = PartialDigestAlgorithm.Distances(candidate1, currPoints)
            If CanRemoveDistances(distances1, currCounts) Then
                ' Create new counts by removing distances
                Dim newCounts1 As New IntDict(currCounts)
                For Each dist As Integer In distances1
                    newCounts1(dist) -= 1
                Next dist
                ' Filter out counts that are zero
                newCounts1 = (From kv In newCounts1 Where kv.Value > 0).ToDictionary(
                    Function(kv) kv.Key, Function(kv) kv.Value)

                ' Create new points list with the candidate
                Dim newPoints1 As New IntList(currPoints)
                If Not newPoints1.Contains(candidate1) Then
                    newPoints1.Add(candidate1)
                    stackOfTuple.Push((newCounts1, newPoints1))
                End If
            End If

            ' Process second candidate point
            Dim distances2 As IntList = PartialDigestAlgorithm.Distances(candidate2, currPoints)
            If CanRemoveDistances(distances2, currCounts) Then
                ' Create new counts by removing distances
                Dim newCounts2 As New IntDict(currCounts)
                For Each dist As Integer In distances2
                    newCounts2(dist) -= 1
                Next dist
                ' Filter out counts that are zero
                newCounts2 = (From kv In newCounts2 Where kv.Value > 0).ToDictionary(
                    Function(kv) kv.Key, Function(kv) kv.Value)

                ' Create new points list with the candidate
                Dim newPoints2 As New IntList(currPoints)
                If Not newPoints2.Contains(candidate2) Then
                    newPoints2.Add(candidate2)
                    stackOfTuple.Push((newCounts2, newPoints2))
                End If
            End If
        Loop

        Return Array.Empty(Of Integer)
    End Function

    ''' <summary>
    ''' Extension method to run the Partial Digest algorithm on an array of fragment lengths.
    ''' </summary>
    ''' <param name="fragmentLengths">An array containing all pairwise distances between points.</param>
    ''' <returns>An ordered array of points that produce the input fragment lengths, or an empty array if no solution exists.</returns>
    <Runtime.CompilerServices.Extension>
    Public Function PartialDigest(fragmentLengths As Integer()) As Integer()
        Dim width As Integer = fragmentLengths.Max()
        fragmentLengths = fragmentLengths.Where(Function(x) x <> width).ToArray()
        Dim starter As New IntList From {0, width}

        Return FindSolution(width, fragmentLengths.ToList(), starter)
    End Function

    ''' <summary>
    ''' Extracts an array of integers from a string formatted as [n1, n2, n3, ...].
    ''' </summary>
    ''' <param name="myContent">The input string in the format [n1, n2, n3, ...].</param>
    ''' <returns>An array of integers parsed from the input string, or an empty array if parsing fails.</returns>
    ''' <exception cref="Exception">Thrown when the input string cannot be parsed into an array of integers.</exception>
    Public Function ExtractArrayFromInput(myContent As String) As Integer()
        Try
            Dim myArray As String() = myContent.Substring(1, myContent.Length - 2).Split(", ")
            Return Array.ConvertAll(myArray, AddressOf Convert.ToInt32)
        Catch ex As Exception
            MessageBox.Show(ex.ToString(), "Your input cannot be resolved for this reason:")
            Return Array.Empty(Of Integer)()
        End Try
    End Function
End Module