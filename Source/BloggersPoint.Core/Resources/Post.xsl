<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" />
  <xsl:template match="/">
    <html>
      <head>
        <script type="text/css">
          body
          {
          font-family:Arial, san-serif;
          font-size:5;
          }
        </script>
      </head>
      <body>
        
          <table border="1" width="100%">
            <col width="10%"/>
         <col width="90%"/>
          <!--<th>Name</th>
          <th>E-Mail</th>
          <th>Phone</th>
          <th>Web-Site</th>-->
          <xsl:for-each select="Post/Author">
            <tr>
              <td>Author:</td>
              <td><xsl:value-of select="Name"/></td>
            </tr>
            <tr>
              <td>E-Mail:</td>
              <td><xsl:value-of select="EMail"/></td>
            </tr>
            <tr>
              <td>Phone No:</td>
              <td><xsl:value-of select="Phone"/></td>
            </tr>
            <tr>
              <td>Web Site:</td>
              <td><xsl:value-of select="Website"/></td>
            </tr>
          </xsl:for-each>
        </table>
      <br></br>
        <br></br>
        <table border="1" width="100%">
         <col width="10%"/>
         <col width="90%"/>
          <xsl:for-each select="Post">
            <tr>
              
              <td>User Id:</td><td><xsl:value-of select="UserId"/>
              </td>
            </tr>
            <tr>
              <td>Post Id:</td><td><xsl:value-of select="PostId"/></td>
            </tr>
            <tr>
              <td>Title:</td>
              <td><xsl:value-of select="Title"/></td>
              </tr>
            <tr>
              <td>Body:</td>
              <td><xsl:value-of select="Body"/></td>
            </tr>
          </xsl:for-each>
        </table>
      <br></br>
        <br></br>
  <table border="1">
          <th>Comment Id</th>
          <th>E-Mail</th>
          <th>Name</th>
          <th>Body</th>
          <xsl:for-each select="Post/Comments/Comment">
            <tr>
              <td>
                <xsl:value-of select="Id"/>
              </td>
              <td>
                <xsl:value-of select="EMail"/>
              </td>
              <td>
                <xsl:value-of select="Name"/>
              </td>
              <td>
                <xsl:value-of select="Body"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
