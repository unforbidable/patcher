<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>

  <xsl:variable name="header">
    <tr>
      <td class="td-name">
        Name
      </td>
      <td class="td-description">
        Description
      </td>
    </tr>
  </xsl:variable>

  <xsl:template match="index | node()">

    <html>
      <head>
        <title>
          Index
        </title>
      </head>
      <body>

        <div class="div-title">
          Index
        </div>

        <xsl:for-each select="category">
          <div class="div-header">
            <xsl:value-of select="@name"/>
          </div>
          <div class="div-table">
            <table>
              <xsl:copy-of select="$header"/>
              <xsl:for-each select="type">
                <tr>
                  <td class="td-name">
                    <see>
                      <xsl:attribute name="cref">
                        <xsl:value-of select="@fullname"/>
                      </xsl:attribute>
                      <xsl:value-of select="@name"/>
                    </see>
                  </td>
                  <td class="td-description">
                    <xsl:copy-of select="summary"/>
                  </td>
                </tr>
              </xsl:for-each>
            </table>
          </div>
        </xsl:for-each>

      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
