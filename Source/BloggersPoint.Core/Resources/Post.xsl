<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" />
  <xsl:template match="/">
    <html>
      <head>
        <script type="text/css">
          body
          {
          font-family:perpetua, Arial, san-serif;
          font-size:5;
          }
        </script>
      </head>
      <body>
        <table border="1">
          <th>UserId</th>
          <th>PostId</th>
          <th>Title</th>
          <th>Body</th>
          <xsl:for-each select="Post">
            <tr>
              <td>
                <xsl:value-of select="UserId"/>
              </td>
              <td>
                <xsl:value-of select="PostId"/>
              </td>
              <td>
                <xsl:value-of select="Title"/>
              </td>
              <td>
                <xsl:value-of select="Body"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>
      <table border="1">
          <th>Name</th>
          <th>E-Mail</th>
          <th>Phone</th>
          <th>Web-Site</th>
          <xsl:for-each select="Post/Author">
            <tr>
              <td>
                <xsl:value-of select="Name"/>
              </td>
              <td>
                <xsl:value-of select="EMail"/>
              </td>
              <td>
                <xsl:value-of select="Phone"/>
              </td>
              <td>
                <xsl:value-of select="Website"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>
      
    
  <table border="1">
          <th>Comment Id</th>
          <th>E-Mail</th>
          <th>Name</th>
          <th>Body</th>
          <xsl:for-each select="Post/Comments/Comments">
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
