Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports MySql.Data.MySqlClient
Public Class Form1
    Dim conn As MySqlConnection
    Dim COMMAND As MySqlCommand

    Private Sub ButtonInsert_Click(sender As Object, e As EventArgs) Handles ButtonInsert.Click
        Dim query As String = "INSERT INTO tracks_tbl (Title, Artist, Duration, Genre) VALUES (@Title, @Artist, @Duration ,@Genre)"
        Try
            Using conn As New MySqlConnection("server=localhost; userid=root; password=root; database=musicstudio_db;")
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@Title", txtTrackTitle.Text)
                    cmd.Parameters.AddWithValue("@Artist", (txtArtist.Text))
                    cmd.Parameters.AddWithValue("@Duration", CInt(txtDuration.Text))
                    cmd.Parameters.AddWithValue("@Genre", cmbGenre.Text)
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Record insert successfully!")
                End Using
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ButtonRead_Click(sender As Object, e As EventArgs) Handles ButtonRead.Click
        Dim query As String = "SELECT * FROM musicstudio_db.tracks_tbl WHERE is_deleted = 0;"
        Try
            Using conn As New MySqlConnection("server=localhost; userid=root; password=root; database=musicstudio_db;")
                Dim adapter As New MySqlDataAdapter(query, conn)
                Dim table As New DataTable()
                adapter.Fill(table)
                DataGridView1.DataSource = table
                DataGridView1.Columns("id").Visible = False
                DataGridView1.Columns("is_deleted").Visible = False
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ButtonEdit_Click(sender As Object, e As EventArgs) Handles ButtonEdit.Click
        Dim query As String = "UPDATE `musicstudio_db`.`tracks_tbl` SET `Title` = @Title, `Artist` = @Artist, `Duration` = @Duration , `Genre` = @Genre WHERE `id`=@id;"
        Try
            Using conn As New MySqlConnection("server=localhost; userid=root; password=root; database=musicstudio_db;")
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@Title", txtTrackTitle.Text)
                    cmd.Parameters.AddWithValue("@Artist", (txtArtist.Text))
                    cmd.Parameters.AddWithValue("@Duration", CInt(txtDuration.Text))
                    cmd.Parameters.AddWithValue("@id", CInt(TextBoxHiddenID.Text))
                    cmd.Parameters.AddWithValue("@Genre", cmbGenre.Text)

                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                    If rowsAffected > 0 Then
                        MessageBox.Show("Record updated successfully!")
                    Else
                        MessageBox.Show("No record found with that ID.")
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ButtonDelete_Click(sender As Object, e As EventArgs) Handles ButtonDelete.Click
        Dim query As String = "UPDATE `musicstudio_db`.`tracks_tbl` SET `is_deleted`=1 WHERE `id`=@id;"
        Try
            Using conn As New MySqlConnection("server=localhost; userid=root; password=root; database=musicstudio_db;")
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("Id", CInt(TextBoxHiddenID.Text))
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Record deleted successfully!")
                    txtTrackTitle.Clear()
                    txtArtist.Clear()
                    txtDuration.Clear()
                    cmbGenre.ResetText()
                End Using
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If e.RowIndex >= 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            txtTrackTitle.Text = selectedRow.Cells("Title").Value.ToString()
            txtArtist.Text = selectedRow.Cells("Artist").Value.ToString()
            txtDuration.Text = selectedRow.Cells("Duration").Value.ToString()
            cmbGenre.Text = selectedRow.Cells("Genre").Value.ToString()

            TextBoxHiddenID.Text = selectedRow.Cells("id").Value.ToString()
        End If
    End Sub
End Class
